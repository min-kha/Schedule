using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ScheduleCore.Entities;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;
using ScheduleService.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace ScheduleService.Service
{
    public class TimetableImporter : ITimetableImporter
    {
        private readonly IInputService _input;
        private readonly ITimetableService _timetable;
        private readonly StudentManagementContext _context;

        public TimetableImporter(IInputService input, ITimetableService timetable, StudentManagementContext context)
        {
            _input = input;
            _timetable = timetable;
            _context = context;
        }

        public async Task<ImportResult<TimetableExtend>> ImportNewScheduleFromFileAsync(string filePath)
        {

            // Đọc file lấy list timetableDto
            IEnumerable<TimetableDto> timetableDtos = await _input.ReadFromFileAsync<TimetableDto>(filePath);

            IEnumerable<TimetableExtend> timetableExtends = _timetable.GenerateScheduleAll(timetableDtos);
            return await SaveToDatabase(timetableExtends);
        }

        private async Task<ImportResult<TimetableExtend>> SaveToDatabase(IEnumerable<TimetableExtend> timetableExtends)
        {
            var importResult = new ImportResult<TimetableExtend>();
            foreach (var timetableExtend in timetableExtends)
            {
                if (timetableExtend.Timetable == null)
                {
                    importResult.ErrorImporteds.Add(timetableExtend);
                    continue;
                }
                try
                {
                    string? conflictMessage = await _timetable.CheckTimetableConflictAsync(timetableExtend.Timetable);
                    if (conflictMessage != null)
                    {
                        timetableExtend.Message = conflictMessage;
                        importResult.ErrorImporteds.Add(timetableExtend);
                    }
                    else
                    {
                        await _context.Timetables.AddAsync(timetableExtend.Timetable);
                        await _context.SaveChangesAsync();
                        timetableExtend.Message = "Đã thêm vào database";
                        importResult.SuccessfullyImporteds.Add(timetableExtend);
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Number == 2627) // Lỗi vi phạm UNIQUE KEY
                        {
                            var sqlExMessage = sqlEx.Message;
                            //// Tạo thông báo lỗi tùy chỉnh
                            var message = GetDuplicateKeyInfo(sqlExMessage);
                            timetableExtend.Message = message;
                            importResult.ErrorImporteds.Add(timetableExtend);
                        }
                    }
                    else
                    {
                        timetableExtend.Message = ex.Message;
                        importResult.ErrorImporteds.Add(timetableExtend);
                    }
                }
                catch (Exception ex)
                {
                    timetableExtend.Message = ex.Message;
                    importResult.ErrorImporteds.Add(timetableExtend);
                }
            }

            return importResult;
        }

        
        public static string GetDuplicateKeyInfo(string errorMessage)
        {
            // Tìm kiếm chuỗi "Violation of UNIQUE KEY constraint"
            var constraintMatch = Regex.Match(errorMessage, @"(?<=Violation of UNIQUE KEY constraint ')(.*?)(?=')");
            if (!constraintMatch.Success)
            {
                return null;
            }

            // Tìm kiếm chuỗi "The duplicate key value is ("
            var duplicateKeyMatch = Regex.Match(errorMessage, @"(?<=The duplicate key value is )(.*?)(?=\.)");
            if (!duplicateKeyMatch.Success)
            {
                return null;
            }

            // Lấy tên ràng buộc UNIQUE KEY
            var constraintName = constraintMatch.Groups[1].Value;

            // Lấy giá trị khóa trùng lặp
            var duplicateKeyValues = duplicateKeyMatch.Groups[1].Value;

            // Trả về thông tin về khóa trùng lặp
            return $"Ràng buộc UNIQUE KEY '{constraintName}' bị vi phạm với giá trị: {duplicateKeyValues}";
        }
    }
}

