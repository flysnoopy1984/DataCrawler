delete from PersonInfo
delete from BookInfo
delete from DataSection
delete from TagInfo

delete from BookTag
delete from BookSeries

delete from [Plan_FromDouBanTagUrls]

select * from PersonInfo
select * from BookInfo order by id desc
select * from DataSection
select * from TagInfo
select * from BookTag
select * from BookSeries
select * from [dbo].[Plan_FromDouBanTagUrls]

select count(1) from BookInfo
--https://book.douban.com/subject/34845963/