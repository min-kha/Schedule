CREATE TABLE [Building] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(255) NOT NULL,
    [description] nvarchar(255) NULL,
    CONSTRAINT [PK_Building] PRIMARY KEY ([id])
);
GO


CREATE TABLE [Slot] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(255) NOT NULL,
    [start] time NOT NULL,
    [end] time NOT NULL,
    CONSTRAINT [PK_Slot] PRIMARY KEY ([id])
);
GO


CREATE TABLE [Student] (
    [id] int NOT NULL IDENTITY,
    [code] nvarchar(255) NOT NULL,
    [name] nvarchar(255) NULL,
    [email] nvarchar(255) NULL,
    [phoneNumber] nvarchar(255) NULL,
    [address] nvarchar(255) NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY ([id])
);
GO


CREATE TABLE [Subject] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(255) NOT NULL,
    [code] varchar(255) NOT NULL,
    [credit] int NULL,
    [description] nvarchar(255) NULL,
    [creditSlot] int NOT NULL,
    CONSTRAINT [PK_Subject] PRIMARY KEY ([id])
);
GO


CREATE TABLE [Teacher] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(255) NOT NULL,
    [email] varchar(255) NOT NULL,
    [phoneNumber] varchar(255) NULL,
    [teacherCode] varchar(255) NOT NULL,
    CONSTRAINT [PK_Teacher] PRIMARY KEY ([id])
);
GO


CREATE TABLE [Room] (
    [id] int NOT NULL IDENTITY,
    [capacity] int NOT NULL DEFAULT (('30')),
    [roomNumber] int NOT NULL,
    [buildingId] int NOT NULL,
    [note] nvarchar(255) NULL,
    CONSTRAINT [PK_Room] PRIMARY KEY ([id]),
    CONSTRAINT [room_buildingid_foreign] FOREIGN KEY ([buildingId]) REFERENCES [Building] ([id])
);
GO


CREATE TABLE [Classroom] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(255) NOT NULL,
    [code] varchar(255) NOT NULL,
    [semesters] int NOT NULL,
    [year] int NOT NULL,
    [subjectId] int NULL,
    CONSTRAINT [PK_Classroom] PRIMARY KEY ([id]),
    CONSTRAINT [classroom_subjectid_foreign] FOREIGN KEY ([subjectId]) REFERENCES [Subject] ([id])
);
GO


CREATE TABLE [Mark] (
    [id] int NOT NULL IDENTITY,
    [subjectId] int NOT NULL,
    [classroomId] int NOT NULL,
    [studentId] int NOT NULL,
    [mark] float NULL,
    [percent] float NULL,
    [name] nvarchar(255) NULL,
    CONSTRAINT [PK_Mark] PRIMARY KEY ([id]),
    CONSTRAINT [mark_classroomid_foreign] FOREIGN KEY ([classroomId]) REFERENCES [Classroom] ([id]),
    CONSTRAINT [mark_studentid_foreign] FOREIGN KEY ([studentId]) REFERENCES [Student] ([id]),
    CONSTRAINT [mark_subjectid_foreign] FOREIGN KEY ([subjectId]) REFERENCES [Subject] ([id])
);
GO


CREATE TABLE [StudentClassroom] (
    [id] int NOT NULL IDENTITY,
    [studentId] int NOT NULL,
    [classroomId] int NOT NULL,
    CONSTRAINT [PK_StudentClassroom] PRIMARY KEY ([id]),
    CONSTRAINT [studentclassroom_classroomid_foreign] FOREIGN KEY ([classroomId]) REFERENCES [Classroom] ([id]),
    CONSTRAINT [studentclassroom_studentid_foreign] FOREIGN KEY ([studentId]) REFERENCES [Student] ([id])
);
GO


CREATE TABLE [Timetable] (
    [id] int NOT NULL IDENTITY,
    [slotId] int NULL,
    [classroomId] int NULL,
    [teacherId] int NULL,
    [roomId] int NULL,
    [subjectId] int NULL,
    [date] date NOT NULL,
    CONSTRAINT [PK_Timetable] PRIMARY KEY ([id]),
    CONSTRAINT [timetable_classid_foreign] FOREIGN KEY ([classroomId]) REFERENCES [Classroom] ([id]),
    CONSTRAINT [timetable_roomid_foreign] FOREIGN KEY ([roomId]) REFERENCES [Room] ([id]),
    CONSTRAINT [timetable_slotid_foreign] FOREIGN KEY ([slotId]) REFERENCES [Slot] ([id]),
    CONSTRAINT [timetable_subjectid_foreign] FOREIGN KEY ([subjectId]) REFERENCES [Subject] ([id]),
    CONSTRAINT [timetable_teacherid_foreign] FOREIGN KEY ([teacherId]) REFERENCES [Teacher] ([id])
);
GO


CREATE TABLE [Attend] (
    [id] int NOT NULL IDENTITY,
    [timeTableId] int NOT NULL,
    [studentId] int NOT NULL,
    [status] int NULL,
    CONSTRAINT [PK_Attend] PRIMARY KEY ([id]),
    CONSTRAINT [attend_studentid_foreign] FOREIGN KEY ([studentId]) REFERENCES [Student] ([id]),
    CONSTRAINT [attend_timetableid_foreign] FOREIGN KEY ([timeTableId]) REFERENCES [Timetable] ([id])
);
GO


CREATE INDEX [IX_Attend_studentId] ON [Attend] ([studentId]);
GO


CREATE INDEX [IX_Attend_timeTableId] ON [Attend] ([timeTableId]);
GO


CREATE UNIQUE INDEX [class_code_unique] ON [Classroom] ([code]);
GO


CREATE INDEX [IX_Classroom_subjectId] ON [Classroom] ([subjectId]);
GO


CREATE INDEX [IX_Mark_classroomId] ON [Mark] ([classroomId]);
GO


CREATE INDEX [IX_Mark_studentId] ON [Mark] ([studentId]);
GO


CREATE INDEX [IX_Mark_subjectId] ON [Mark] ([subjectId]);
GO


CREATE INDEX [IX_Room_buildingId] ON [Room] ([buildingId]);
GO


CREATE UNIQUE INDEX [room_unique_roomNumber] ON [Room] ([roomNumber]);
GO


CREATE UNIQUE INDEX [slot_name_unique] ON [Slot] ([name]);
GO


CREATE UNIQUE INDEX [student_code_unique] ON [Student] ([code]);
GO


CREATE INDEX [IX_StudentClassroom_classroomId] ON [StudentClassroom] ([classroomId]);
GO


CREATE INDEX [IX_StudentClassroom_studentId] ON [StudentClassroom] ([studentId]);
GO


CREATE UNIQUE INDEX [subject_code_unique] ON [Subject] ([code]);
GO


CREATE UNIQUE INDEX [teacher_email_unique] ON [Teacher] ([email]);
GO


CREATE UNIQUE INDEX [teacher_teacherCode_unique] ON [Teacher] ([teacherCode]);
GO


CREATE INDEX [IX_Timetable_classroomId] ON [Timetable] ([classroomId]);
GO


CREATE INDEX [IX_Timetable_roomId] ON [Timetable] ([roomId]);
GO


CREATE INDEX [IX_Timetable_slotId] ON [Timetable] ([slotId]);
GO


CREATE INDEX [IX_Timetable_subjectId] ON [Timetable] ([subjectId]);
GO


CREATE INDEX [IX_Timetable_teacherId] ON [Timetable] ([teacherId]);
GO


CREATE UNIQUE INDEX [timetable_unique_class_per_teacher_slot] ON [Timetable] ([date], [slotId], [classroomId]) WHERE [slotId] IS NOT NULL AND [classroomId] IS NOT NULL;
GO


CREATE UNIQUE INDEX [timetable_unique_room_per_day_slot] ON [Timetable] ([date], [slotId], [roomId]) WHERE [slotId] IS NOT NULL AND [roomId] IS NOT NULL;
GO


CREATE UNIQUE INDEX [timetable_unique_teacher_per_day_slot] ON [Timetable] ([date], [slotId], [teacherId]) WHERE [slotId] IS NOT NULL AND [teacherId] IS NOT NULL;
GO