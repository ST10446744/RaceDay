/*
    RaceDay Event Management System
    Portfolio of Evidence - Part 1
    SQL Server Database Script

    This script:
    1. Creates the RaceDay database
    2. Creates all database tables
    3. Creates primary and foreign keys
    4. Applies validation constraints
    5. Inserts sample data
*/

USE master;
GO

/* ---------------------------------------------------------
   1. CREATE DATABASE
   --------------------------------------------------------- */

IF DB_ID('RaceDay') IS NOT NULL
BEGIN
    ALTER DATABASE RaceDay
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;

    DROP DATABASE RaceDay;
END
GO

CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

/* ---------------------------------------------------------
   2. USERS
   --------------------------------------------------------- */

CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    DateRegistered DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO

/* ---------------------------------------------------------
   3. EVENTS
   --------------------------------------------------------- */

CREATE TABLE Events
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(1000) NULL,
    EventType NVARCHAR(30) NOT NULL,
    EventDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    Venue NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Province NVARCHAR(100) NOT NULL,
    DistanceKM DECIMAL(6,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL DEFAULT 0,
    MaximumParticipants INT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Upcoming',
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Events_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES Users(UserID),

    CONSTRAINT CK_Events_Type
        CHECK (EventType IN ('Running', 'Walking', 'Cycling')),

    CONSTRAINT CK_Events_Distance
        CHECK (DistanceKM > 0),

    CONSTRAINT CK_Events_EntryFee
        CHECK (EntryFee >= 0),

    CONSTRAINT CK_Events_MaxParticipants
        CHECK (
            MaximumParticipants IS NULL
            OR MaximumParticipants > 0
        ),

    CONSTRAINT CK_Events_Status
        CHECK (
            Status IN
            ('Upcoming', 'Open', 'Closed', 'Completed', 'Cancelled')
        )
);
GO

/* ---------------------------------------------------------
   4. CATEGORIES
   --------------------------------------------------------- */

CREATE TABLE Categories
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    AgeMinimum INT NULL,
    AgeMaximum INT NULL,
    CategoryFee DECIMAL(10,2) NOT NULL DEFAULT 0,

    CONSTRAINT FK_Categories_Event
        FOREIGN KEY (EventID)
        REFERENCES Events(EventID)
        ON DELETE CASCADE,

    CONSTRAINT CK_Categories_AgeMinimum
        CHECK (
            AgeMinimum IS NULL
            OR AgeMinimum >= 0
        ),

    CONSTRAINT CK_Categories_AgeMaximum
        CHECK (
            AgeMaximum IS NULL
            OR AgeMaximum >= 0
        ),

    CONSTRAINT CK_Categories_AgeRange
        CHECK (
            AgeMaximum IS NULL
            OR AgeMinimum IS NULL
            OR AgeMaximum >= AgeMinimum
        ),

    CONSTRAINT CK_Categories_Fee
        CHECK (CategoryFee >= 0),

    CONSTRAINT UQ_Categories_Event_Category
        UNIQUE (EventID, CategoryName)
);
GO

/* ---------------------------------------------------------
   5. ENROLMENTS
   --------------------------------------------------------- */

CREATE TABLE Enrolments
(
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    EmergencyContactName NVARCHAR(100) NOT NULL,
    EmergencyContactPhone NVARCHAR(20) NOT NULL,
    PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    EnrolmentStatus NVARCHAR(20) NOT NULL DEFAULT 'Active',

    CONSTRAINT FK_Enrolments_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES Users(UserID),

    CONSTRAINT FK_Enrolments_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Categories(CategoryID),

    CONSTRAINT CK_Enrolments_PaymentStatus
        CHECK (
            PaymentStatus IN
            ('Pending', 'Paid', 'Refunded')
        ),

    CONSTRAINT CK_Enrolments_Status
        CHECK (
            EnrolmentStatus IN
            ('Active', 'Cancelled')
        ),

    CONSTRAINT UQ_Enrolments_Participant_Category
        UNIQUE (ParticipantID, CategoryID)
);
GO

/* ---------------------------------------------------------
   6. RESULTS
   --------------------------------------------------------- */

CREATE TABLE Results
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL,
    FinishPosition INT NULL,
    FinishTime TIME NULL,
    ResultStatus NVARCHAR(20) NOT NULL DEFAULT 'Finished',
    RecordedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Results_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolments(EnrolmentID)
        ON DELETE CASCADE,

    CONSTRAINT CK_Results_Position
        CHECK (
            FinishPosition IS NULL
            OR FinishPosition > 0
        ),

    CONSTRAINT CK_Results_Status
        CHECK (
            ResultStatus IN
            ('Finished', 'DNF', 'DNS', 'Disqualified')
        ),

    CONSTRAINT UQ_Results_Enrolment
        UNIQUE (EnrolmentID)
);
GO

/* ---------------------------------------------------------
   7. EVENT DOCUMENTS
   --------------------------------------------------------- */

CREATE TABLE EventDocuments
(
    DocumentID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    DocumentName NVARCHAR(200) NOT NULL,
    BlobFileName NVARCHAR(300) NOT NULL,
    DocumentType NVARCHAR(100) NULL,
    UploadedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_EventDocuments_Event
        FOREIGN KEY (EventID)
        REFERENCES Events(EventID)
        ON DELETE CASCADE
);
GO

/* ---------------------------------------------------------
   8. PAYMENTS
   --------------------------------------------------------- */

CREATE TABLE Payments
(
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME2 NULL,
    PaymentMethod NVARCHAR(30) NULL,
    TransactionReference NVARCHAR(100) NULL UNIQUE,
    PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',

    CONSTRAINT FK_Payments_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolments(EnrolmentID),

    CONSTRAINT CK_Payments_Amount
        CHECK (Amount >= 0),

    CONSTRAINT CK_Payments_Method
        CHECK (
            PaymentMethod IS NULL
            OR PaymentMethod IN
            ('Card', 'EFT', 'Cash', 'Online')
        ),

    CONSTRAINT CK_Payments_Status
        CHECK (
            PaymentStatus IN
            ('Pending', 'Successful', 'Failed', 'Refunded')
        )
);
GO

/* ---------------------------------------------------------
   9. WEATHER INFORMATION
   --------------------------------------------------------- */

CREATE TABLE WeatherInformation
(
    WeatherID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    ForecastDate DATE NOT NULL,
    TemperatureC DECIMAL(5,2) NULL,
    Condition NVARCHAR(100) NULL,
    WindSpeedKmh DECIMAL(6,2) NULL,
    RainProbability DECIMAL(5,2) NULL,
    RetrievedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Weather_Event
        FOREIGN KEY (EventID)
        REFERENCES Events(EventID)
        ON DELETE CASCADE,

    CONSTRAINT CK_Weather_WindSpeed
        CHECK (
            WindSpeedKmh IS NULL
            OR WindSpeedKmh >= 0
        ),

    CONSTRAINT CK_Weather_RainProbability
        CHECK (
            RainProbability IS NULL
            OR (
                RainProbability >= 0
                AND RainProbability <= 100
            )
        ),

    CONSTRAINT UQ_Weather_Event_Date
        UNIQUE (EventID, ForecastDate)
);
GO

/* ---------------------------------------------------------
   10. SAMPLE USERS
   --------------------------------------------------------- */

INSERT INTO Users
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    Role,
    PhoneNumber
)
VALUES
(
    'Thabo',
    'Mokoena',
    'thabo.mokoena@raceday.co.za',
    'HASH_ORGANISER_001',
    'Organiser',
    '0821111111'
),
(
    'Lerato',
    'Naidoo',
    'lerato.naidoo@raceday.co.za',
    'HASH_ORGANISER_002',
    'Organiser',
    '0822222222'
),
(
    'Sipho',
    'Dlamini',
    'sipho.dlamini@example.com',
    'HASH_PARTICIPANT_001',
    'Participant',
    '0833333333'
),
(
    'Ayesha',
    'Khan',
    'ayesha.khan@example.com',
    'HASH_PARTICIPANT_002',
    'Participant',
    '0844444444'
),
(
    'Daniel',
    'Jacobs',
    'daniel.jacobs@example.com',
    'HASH_PARTICIPANT_003',
    'Participant',
    '0855555555'
),
(
    'Naledi',
    'Molefe',
    'naledi.molefe@example.com',
    'HASH_PARTICIPANT_004',
    'Participant',
    '0866666666'
);
GO

/* ---------------------------------------------------------
   11. SAMPLE EVENTS
   --------------------------------------------------------- */

INSERT INTO Events
(
    OrganiserID,
    EventName,
    Description,
    EventType,
    EventDate,
    StartTime,
    Venue,
    City,
    Province,
    DistanceKM,
    EntryFee,
    MaximumParticipants,
    Status
)
VALUES
(
    1,
    'Johannesburg City Run 2026',
    'Annual road running event through Johannesburg.',
    'Running',
    '2026-10-18',
    '07:00',
    'Mary Fitzgerald Square',
    'Johannesburg',
    'Gauteng',
    21.10,
    250.00,
    5000,
    'Open'
),
(
    1,
    'Pretoria Spring Walk 2026',
    'Community walking event in Pretoria.',
    'Walking',
    '2026-11-08',
    '07:30',
    'Union Buildings',
    'Pretoria',
    'Gauteng',
    10.00,
    120.00,
    3000,
    'Open'
),
(
    2,
    'Cape Town Coastal Cycle 2026',
    'Coastal cycling event around Cape Town.',
    'Cycling',
    '2026-12-06',
    '06:30',
    'Cape Town Stadium',
    'Cape Town',
    'Western Cape',
    42.00,
    450.00,
    4000,
    'Upcoming'
);
GO

/* ---------------------------------------------------------
   12. SAMPLE CATEGORIES
   --------------------------------------------------------- */

INSERT INTO Categories
(
    EventID,
    CategoryName,
    Description,
    AgeMinimum,
    AgeMaximum,
    CategoryFee
)
VALUES
(1, 'Open Men', 'Open male category', 18, NULL, 250.00),
(1, 'Open Women', 'Open female category', 18, NULL, 250.00),
(1, 'Junior', 'Junior running category', 14, 17, 150.00),

(2, 'Open Walk', 'Open walking category', 18, NULL, 120.00),
(2, 'Junior Walk', 'Junior walking category', 13, 17, 80.00),

(3, 'Open Men', 'Open male cycling category', 18, NULL, 450.00),
(3, 'Open Women', 'Open female cycling category', 18, NULL, 450.00),
(3, 'Veterans', 'Veteran cycling category', 40, NULL, 400.00);
GO

/* ---------------------------------------------------------
   13. SAMPLE ENROLMENTS
   --------------------------------------------------------- */

INSERT INTO Enrolments
(
    ParticipantID,
    CategoryID,
    EmergencyContactName,
    EmergencyContactPhone,
    PaymentStatus,
    EnrolmentStatus
)
VALUES
(
    3,
    1,
    'Maria Dlamini',
    '0871111111',
    'Paid',
    'Active'
),
(
    4,
    2,
    'Ahmed Khan',
    '0872222222',
    'Paid',
    'Active'
),
(
    5,
    4,
    'Sarah Jacobs',
    '0873333333',
    'Pending',
    'Active'
),
(
    6,
    6,
    'Peter Molefe',
    '0874444444',
    'Paid',
    'Active'
),
(
    3,
    8,
    'Maria Dlamini',
    '0871111111',
    'Paid',
    'Active'
);
GO

/* ---------------------------------------------------------
   14. SAMPLE PAYMENTS
   --------------------------------------------------------- */

INSERT INTO Payments
(
    EnrolmentID,
    Amount,
    PaymentDate,
    PaymentMethod,
    TransactionReference,
    PaymentStatus
)
VALUES
(
    1,
    250.00,
    '2026-08-15',
    'Online',
    'RDAY-PAY-0001',
    'Successful'
),
(
    2,
    250.00,
    '2026-08-16',
    'Card',
    'RDAY-PAY-0002',
    'Successful'
),
(
    4,
    450.00,
    '2026-08-20',
    'EFT',
    'RDAY-PAY-0003',
    'Successful'
),
(
    5,
    450.00,
    '2026-08-21',
    'Online',
    'RDAY-PAY-0004',
    'Successful'
);
GO

/* ---------------------------------------------------------
   15. SAMPLE EVENT DOCUMENTS
   --------------------------------------------------------- */

INSERT INTO EventDocuments
(
    EventID,
    DocumentName,
    BlobFileName,
    DocumentType
)
VALUES
(
    1,
    'Johannesburg City Run Route Map',
    'joburg-city-run-route.pdf',
    'application/pdf'
),
(
    2,
    'Pretoria Spring Walk Information',
    'pretoria-spring-walk-info.pdf',
    'application/pdf'
),
(
    3,
    'Cape Town Coastal Cycle Route',
    'cape-town-cycle-route.pdf',
    'application/pdf'
);
GO

/* ---------------------------------------------------------
   16. SAMPLE WEATHER INFORMATION
   --------------------------------------------------------- */

INSERT INTO WeatherInformation
(
    EventID,
    ForecastDate,
    TemperatureC,
    Condition,
    WindSpeedKmh,
    RainProbability
)
VALUES
(
    1,
    '2026-10-18',
    19.50,
    'Partly Cloudy',
    14.00,
    20.00
),
(
    2,
    '2026-11-08',
    21.00,
    'Sunny',
    11.00,
    10.00
),
(
    3,
    '2026-12-06',
    23.50,
    'Clear',
    18.00,
    15.00
);
GO

/* ---------------------------------------------------------
   17. VERIFICATION
   --------------------------------------------------------- */

SELECT 'Users' AS TableName, COUNT(*) AS RecordCount
FROM Users

UNION ALL

SELECT 'Events', COUNT(*)
FROM Events

UNION ALL

SELECT 'Categories', COUNT(*)
FROM Categories

UNION ALL

SELECT 'Enrolments', COUNT(*)
FROM Enrolments

UNION ALL

SELECT 'Results', COUNT(*)
FROM Results

UNION ALL

SELECT 'EventDocuments', COUNT(*)
FROM EventDocuments

UNION ALL

SELECT 'Payments', COUNT(*)
FROM Payments

UNION ALL

SELECT 'WeatherInformation', COUNT(*)
FROM WeatherInformation;
GO