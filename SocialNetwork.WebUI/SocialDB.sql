create database SocialDB

use SocialDB

CREATE TABLE Users(
	Id int primary key identity(1,1) not null,
	ImageURL nvarchar(max),
	BgImageURL nvarchar(max),
	FirstName nvarchar(50),
	Lastname nvarchar(50),
	Email nvarchar(max) not null,
	PhoneNumber nvarchar(40),
	Country nvarchar(100),
	[Address] nvarchar(200),
	TownOrCity nvarchar(100),
	PostCode int,
	[Description] nvarchar(max),
	Username nvarchar(50) not null unique,
	[Password] nvarchar(200) not null,
	Follow int not null default(0),
	[Following] int not null default(0),
	IsLogined bit
)

Create Table [Notifications](
	[Id] int primary key identity(1,1) not null,
	[SenderUserId] int not null foreign key references Users(Id),
	[ReceiveUserId] int not null foreign key references Users(Id),
	[SendDate] datetime2 not null default(sysdatetime()),
	Title nvarchar(max) not null,
	[Message] nvarchar(max) null)

Create Table Friends(
	[Id] int primary key identity(1,1) not null,
	[FriendId] int not null foreign key references Users(Id),
	[UserId] int not null foreign key references Users(Id))



Create table [Groups](
[Id] int primary key identity(1,1) not null,
[Name] nvarchar(30) not null,
)

Create Table [Chats](
	[Id] int primary key identity(1,1) not null,
	[SenderUserId] int not null foreign key references Users(Id),
	[AcceptUserId] int not null foreign key references Users(Id),
	[DateNot] datetime2 not null default(sysdatetime()),
	[Message] nvarchar(max) not null
)

create table GroupChats(
	[Id] int primary key identity(1,1) not null,
	[GroupId] int foreign key references Groups(Id),
	[ImageURL] nvarchar(max),
	[Message] nvarchar(max)
)



CREATE TABLE Posts(
	Id int primary key identity(1,1) not null,
	IdUser int not null foreign key references Users(Id),
	[Message] nvarchar(max),
	[PostImage] nvarchar(max),
	DatePost datetime2 not null default(sysdatetime()),
	Rating float not null default(0)
)

create table Histories(
	[Id] int primary key identity(1,1) not null,
	[UserId] int foreign key references Users(Id) not null,
	[HistoryData] nvarchar(max) not null
)

create table Comments(
	Id int primary key identity(1,1) not null,
	IdUser int not null foreign key references Users(Id),
	IdPost int not null foreign key references Posts(Id),
	[Message] nvarchar(max) not null,
	DateComment datetime2 not null default(sysdatetime()),
	Rating float not null default(0)
)

create table PostRating(
	IdPost int not null foreign key references Posts(Id),
	IdUser int not null foreign key references Users(Id),
	Mark int not null,
	constraint UQ_PostRating unique(IdPost,IdUser)
)

create table CommentRating(
	IdComment int not null foreign key references Comments(Id),
	IdUser int not null foreign key references Users(Id),
	Mark int not null,
	constraint UQ_CommentRating unique (IdComment,IdUser)
)
