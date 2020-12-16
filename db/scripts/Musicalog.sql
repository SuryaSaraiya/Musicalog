USE Musicalog
GO


IF OBJECT_ID('album_artists') IS NOT NULL
BEGIN
	DROP TABLE album_artists
END

IF OBJECT_ID('albums') IS NOT NULL
BEGIN
	DROP TABLE albums
END

IF OBJECT_ID('album_types') IS NOT NULL
BEGIN
	DROP TABLE album_types
END

CREATE TABLE album_types
(
	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Type] VARCHAR(20) NOT NULL
)

IF OBJECT_ID('album_artists') IS NOT NULL
BEGIN
	DROP TABLE album_artists
END

IF OBJECT_ID('albums') IS NOT NULL
BEGIN 
	DROP TABLE albums
END 
CREATE TABLE albums
(
	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	album_name NVARCHAR(256),
	sku VARCHAR(20) NOT NULL,
	[type_id] INT NOT NULL,
	FOREIGN KEY ([type_id]) REFERENCES album_types(id)
)

IF OBJECT_ID('artists') IS NOT NULL
BEGIN
	DROP TABLE artists
END

CREATE TABLE artists
(
	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	full_name NVARCHAR(256) NOT NULL
)

CREATE TABLE album_artists
(
	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	album_id INT,
	artist_id INT,
	FOREIGN KEY(album_id) REFERENCES albums(id),
	FOREIGN KEY(artist_id) REFERENCES artists(id)
)

IF OBJECT_ID('inventory') IS NOT NULL
BEGIN
	DROP TABLE inventory
END

CREATE TABLE inventory
(
	id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	sku VARCHAR(20) NOT NULL,
	stock INT not null,
	sold INT NOT NULL
)

INSERT INTO album_types VALUES ('Vinyl')
INSERT INTO album_types VALUES ('CD')

INSERT INTO albums values ('Reckless','ABMV-IDHN',1)
INSERT INTO artists values ('Brian Adams')
INSERT INTO album_artists values (1,1)
INSERT INTO inventory values ('ABMV-IDHN',2000,1025)


INSERT INTO albums values ('Desperado','ABMC-EUEN',2)
INSERT INTO artists values ('Eagles')
INSERT INTO album_artists values (2,2)
INSERT INTO inventory values ('ABMC-EUEN',4000,3247)

SELECT * FROM albums
SELECT * FROM artists
SELECT * FROM inventory
select * from album_artists