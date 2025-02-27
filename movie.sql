-- bảng người dùng
create table Users (
UserID int IDENTITY(1,1) PRIMARY KEY,
Username nvarchar (50) not null unique,
Email nvarchar (100)  not null unique,
Password nvarchar (255) not null,
Createdat datetime default getdate()
);

--Bảng đạo diễn
create table Directors (
DirectorsID int identity(1,1) primary key,
NameDir nvarchar (225) not null,
Description nvarchar (max),
Nationlity nvarchar (100),
Professional nvarchar (255),
);

--bảng diễn viên
create table Actors (
ActorsID int identity(1,1) primary key,
NameAct nvarchar (225) not null,
Description nvarchar (max),
Nationlity nvarchar (100),
Professional nvarchar (255),
);

--bảng thể loại
create table Categories(
CategoriesID int identity(1,1) primary key,
CategoryName nvarchar (50) not null unique,
);

-- Bảng Movies
create table Movies(
MovieID int identity(1,1) Primary key,
Title nvarchar (255) not null,
CategoriesID int,
Description nvarchar (max),
DirectorID int,
ActorsID int,
Rating Decimal (3,1),
PosterURL nvarchar (255), 
AvartarURl nvarchar (255),
LinkFilmURl nvarchar (255),
Foreign key (CategoriesID) references Categories (CategoriesID)

);

-- gói đăng ký
create table Subscrip(
SubscripID int identity(1,1) Primary key,
UserID int,
PlanName nvarchar (50),
Price decimal (10,2),
StartDate date,
EndDate date,
Status int ,
Foreign key (UserID) references Users(UserID)
);

-- Lịch sử xem
create table WatchHistory(
HistoryID int identity(1,1) Primary key,
UserID int,
MovieID int,
Foreign key (UserID) references Users (UserID),
Foreign key (MovieID) references  Movies(MovieID),
);

--Bảng liên kết n-n phim- thể loại
create table MovieCategories(
MovieID int,
CategoriesID int,
Primary key(MovieID, CategoriesID),
Foreign key (MovieID) references  Movies(MovieID) on delete cascade,
Foreign key (CategoriesID) references  Categories (CategoriesID) on delete cascade,
);

--Bảng Liên kết n-n phim-diễn viên
create table MovieActor(
MovieID int,
ActorsID int,
Primary key(MovieID,ActorsID),
Foreign key (MovieID) references  Movies(MovieID) on delete cascade,
Foreign key (ActorsID) references  Actors(ActorsID) on delete cascade,
);

-- Bảng liên kêt n-n phim- đạo diễn
create table MovieDir(
MovieID int,
DirectorsID int,
Primary key (MovieID, DirectorsID),
foreign key (MovieID) references  Movies(MovieID) on delete cascade,
Foreign key (DirectorsID) references  Directors(DirectorsID) on delete cascade
);


