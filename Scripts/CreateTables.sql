-- CREATE TABLE commands
-- INDEX creation commands
-- CONSTRAINT creation commands

-- SERIAL: automatically incremented values
-- RETURNING: returns new values from INSERT/UPDATE operations
-- TEXT: for unlimited textual values
-- BOOLEAN: bool values

-- IF NOT EXISTS: if table exists skips the command


-- About
-- Skills
-- Projects
-- Testimonials
-- Contacts

-- About Table
CREATE TABLE IF NOT EXISTS About(
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    ImageUrl VARCHAR(500),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP
);

-- Skills Table
CREATE TABLE IF NOT EXISTS Skills(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Percentage INTEGER NOT NULL CHECK (Percentage >= 0 AND Percentage <= 100),
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

--Projects Table
CREATE TABLE IF NOT EXISTS Projects(
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    ImageUrl VARCHAR(500),
    ProjectUrl VARCHAR(500),
    GithubUrl VARCHAR(500),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsActive BOOLEAN
);

--Testimonials Table
CREATE TABLE IF NOT EXISTS Testimonials(
    Id SERIAL PRIMARY KEY,
    ClientName VARCHAR(100) NOT NULL,
    ClientPosition VARCHAR(100),
    Comment TEXT NOT NULL,
    ClientImageUrl VARCHAR(500),
    Rating INTEGER NOT NULL CHECK(Rating >= 1 AND Rating <= 5),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsActive BOOLEAN
);

--Contacts Table
CREATE TABLE IF NOT EXISTS Contacts(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Subject VARCHAR(200),
    Message TEXT NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsRead BOOLEAN
);

