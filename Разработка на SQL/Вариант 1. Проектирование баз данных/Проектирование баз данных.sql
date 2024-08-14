-- Содержится информация о названии курса и описание курса описание.
CREATE TABLE Courses (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Description TEXT -- Описание для курса.
);
-- Содержится информация об обучающихся, их имя, фамилия и электронная почта(Не должна повторяться).
CREATE TABLE Trainee (
    ID INT PRIMARY KEY IDENTITY(1,1),
    First_name VARCHAR(50) NOT NULL,
    Last_name VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL
);
-- Содержится информация о выбранном курсе, колличество мест на курс, дата начала и конца курса. 
CREATE TABLE Schedule (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Courses_id INT NOT NULL,
    Number_of_places INT NOT NULL,
    Start_date DATE NOT NULL,
    End_date DATE NOT NULL,
    FOREIGN KEY (Courses_id) REFERENCES Courses(ID)
);
-- Содержится информация о записях. ID обучающегося, ID расписания и дата записи на курс. Обучающийся не может посетить один курс дважды.
CREATE TABLE Enrollments (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Trainee_ID INT NOT NULL,
    Schedule_id INT NOT NULL,
    Enrollments_date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Trainee_ID) REFERENCES Trainee(ID),
    FOREIGN KEY (Schedule_id) REFERENCES Schedule(ID),
    CONSTRAINT Unique_Enrollment UNIQUE (Trainee_id, Schedule_id)  -- Составной уникальный индекс, потому что обучаемый не может постетить один и тот же курс дважды. 
);