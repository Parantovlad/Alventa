CREATE PROCEDURE spGetSalesReport
	@From date,
	@To date
AS
SELECT Q1.FIO, Q1.[Quantity Orders], Q2.[Sold Items], Q1.[Quantity Customers], Q2.[Order Sum], Q2.[Average Cost Of Product], Q2.[Order Average], Q2.[Average Sum On Customer] FROM
	(SELECT 
	LastName + FirstName as FIO, 
	COUNT(o.OrderID) as [Quantity Orders], 
	COUNT(DISTINCT o.CustomerID) as [Quantity Customers]
	FROM Employees as e
	INNER JOIN Orders as o ON o.EmployeeID = e.EmployeeID
	WHERE o.OrderDate >= @From AND o.OrderDate <= @To
	GROUP BY LastName + FirstName) Q1,
	(SELECT 
	LastName + FirstName as FIO, 
	SUM(od.Quantity) as [Sold Items],
	SUM(od.Quantity * od.UnitPrice) as [Order Sum],
	AVG(od.UnitPrice) as [Average Cost Of Product],
	SUM(od.Quantity * od.UnitPrice) / COUNT(DISTINCT o.OrderID) as [Order Average],
	SUM(od.Quantity * od.UnitPrice) / COUNT(DISTINCT o.CustomerID) as [Average Sum On Customer]
	FROM Employees as e
	INNER JOIN Orders as o ON o.EmployeeID = e.EmployeeID
	INNER JOIN [Order Details] as od ON od.OrderID = o.OrderID
	WHERE o.OrderDate >= @From AND o.OrderDate <= @To
	GROUP BY LastName + FirstName) Q2
WHERE Q1.FIO = Q2.FIO;
GO

EXECUTE spGetSalesReport '19960704', '19970805'
GO