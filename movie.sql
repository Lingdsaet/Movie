use movieDB;



-- bảng người dùng
create table Users (
UserID int IDENTITY(1,1) PRIMARY KEY,
Username nvarchar (50) not null unique,
Email nvarchar (100)  not null unique,
Password nvarchar (255) not null,
Createdat datetime default getdate()
Status int default 1 ,
);

--Bảng đạo diễn
create table Directors (
DirectorID int identity(1,1) primary key,
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

-- Bảng Movies phim lẻ
create table Movies(
MovieID int identity(1,1) Primary key,
Title nvarchar (255) not null,
Description nvarchar (max),
DirectorID int,
Rating Decimal (3,1),
PosterURL nvarchar (255), 
AvatarURl nvarchar (255),
LinkFilmURl nvarchar (255),
Status int default 1 ,
Foreign key (DirectorID) references  Directors (DirectorID)
);

--Bảng movie phim bộ
create table Series(
SeriesID int identity(1,1) Primary key,
Title nvarchar (255) not null,
Description nvarchar (max),
DirectorID int,
Rating Decimal (3,1),
Season int not null default 1,
PosterURL nvarchar (255),
AvatarURl nvarchar (255),
Foreign key (DirectorID) references  Directors (DirectorID)
);

--bảng quản lý tập phim
create table Episodes(
EpisodeID int identity(1,1) Primary key,
SeriesID int,
EpisodeNumber int not null,
Title nvarchar (255),
-- Duration int,
LinkFilmURL nvarchar (255) not null,
Foreign key (SeriesID) references Series (SeriesID)
Status int default 1 ,
);

-- gói đăng ký
create table Payment(
SubpaymentID int identity(1,1) Primary key,
UserID int,
PlanName nvarchar (50),
Price decimal (10,2),
StartDate date null,
EndDate date null,
Status int null,
Foreign key (UserID) references Users(UserID)
Status int default 1 ,
);


--Bảng liên kết n-n phim- thể loại
create table MovieCategories(
MovieID int,
CategoriesID int,
Primary key(MovieID, CategoriesID),
Foreign key (MovieID) references  Movies(MovieID) on delete cascade,
Foreign key (CategoriesID) references  Categories (CategoriesID) on delete cascade,
);

create table SeriesCategories(
SeriesID int,
CategoriesID int,
Primary key(SeriesID, CategoriesID),
Foreign key (SeriesID) references  Series(SeriesID) on delete cascade,
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

create table SeriesActor(
SeriesID int,
ActorsID int,
Primary key(SeriesID,ActorsID),
Foreign key (SeriesID) references Series(SeriesID) on delete cascade,
Foreign key (ActorsID) references  Actors(ActorsID) on delete cascade,
);






-- chèn dữ liệu mẫu
Declare @i int =1
while @i <= 10
begin 
	insert into Users (Username, Email, Password)
	values ('User' + CAST(@i as nvarchar), 'user'+ CAST(@i as nvarchar) + '@example.com', 'password' + CAST(@i as nvarchar));
	
	insert into Directors (NameDir, Description, Nationlity, Professional)
	values ('Directors' + CAST(@i as nvarchar),'Nothing' ,'country'+ CAST(@i as nvarchar), 'Directors');

	insert into Actors(NameAct, Description, Nationlity, Professional)
	values ('Actors' + CAST(@i as nvarchar),'Nothing' ,'country'+ CAST(@i as nvarchar), 'Actors');

	-- Chèn phim lẻ
    INSERT INTO Movies (Title, Description, DirectorID, Rating, PosterURL, AvatarURL, LinkFilmURL)
    VALUES ('Movie' + CAST(@i AS NVARCHAR),  'Movie description ' + CAST(@i AS NVARCHAR), @i, 7.5, 'poster' + CAST(@i AS NVARCHAR) + '.jpg', 'avatar' + CAST(@i AS NVARCHAR) + '.jpg', 'movie' + CAST(@i AS NVARCHAR) + '.mp4');
    
    -- Chèn phim bộ
    INSERT INTO Series (Title, Description, DirectorID, Rating, PosterURL, AvatarURL, Season)
    VALUES ('Series' + CAST(@i AS NVARCHAR), 'Series description ' + CAST(@i AS NVARCHAR), @i, 8.0, 'poster' + CAST(@i AS NVARCHAR) + '.jpg', 'avatar' + CAST(@i AS NVARCHAR) + '.jpg',1);
    
    -- Chèn tập phim
    INSERT INTO Episodes (SeriesID, EpisodeNumber, Title, LinkFilmURL)
    VALUES (@i, 1, 'Episode 1 of Series' + CAST(@i AS NVARCHAR), 'episode' + CAST(@i AS NVARCHAR) + '.mp4');
	SET @i = @i +1;
end 
   
-- dữ liệu thể loại
INSERT INTO Categories (CategoryName) VALUES
('Action'),
('Drama'),
('Comedy'),
('Horror'),
('Science Fiction'),
('Romance'),
('Adventure'),
('Fantasy'),
('Thriller'),
('Animation');


INSERT INTO Payment (UserID, PlanName, Price, StartDate, EndDate, Status) VALUES
(1, 'Basic Plan', 5.99, '2024-01-01', '2024-06-30', 1),
(2, 'Standard Plan', 9.99, '2024-02-01', '2024-07-31', 1),
(3, 'Premium Plan', 14.99, '2024-03-01', '2024-08-31', 1),
(4, 'Family Plan', 19.99, '2024-04-01', '2024-09-30', 1),
(5, 'Student Plan', 4.99, '2024-05-01', '2024-10-31', 1);
