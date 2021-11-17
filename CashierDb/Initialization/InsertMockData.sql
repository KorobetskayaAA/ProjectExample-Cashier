USE [Cashier];
GO

INSERT INTO BillStatus ([Name]) 
VALUES (N'Открыт')
      ,(N'Закрыт')
      ,(N'Отмена');
GO

INSERT INTO Product (Barcode, [Name], Price)
VALUES (4634567890098, 'Батон горчичный', 37.12),
       (8001234567891, 'Спагетти Италия 450г', 89.9),
       (4609876541212, 'Вода негаз. 0,5л', 15.6),
       (5345738573637, 'Томаты вес.', 90.5),
       (1234567891234, 'Огурцы вес.', 75.25);
GO


INSERT INTO Bill ([Status]) VALUES (1);
DECLARE @lastBillNumber BIGINT;
SET @lastBillNumber = (SELECT MAX(Number) FROM Bill);
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, Barcode, [Name], Price, 1 FROM Product WHERE Barcode = 4634567890098;
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 0.854 FROM Product WHERE Barcode = 5345738573637;
GO

INSERT INTO Bill ([Status]) VALUES (2);
DECLARE @lastBillNumber BIGINT;
SET @lastBillNumber = (SELECT MAX(Number) FROM Bill);
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 2 FROM Product WHERE Barcode = 4609876541212;
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 1 FROM Product WHERE Barcode = 8001234567891;
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 1.045 FROM Product WHERE Barcode = 5345738573637;
GO

INSERT INTO Bill ([Status]) VALUES (1);
DECLARE @lastBillNumber BIGINT;
SET @lastBillNumber = (SELECT MAX(Number) FROM Bill);
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 2 FROM Product WHERE Barcode = 4609876541212;
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 1 FROM Product WHERE Barcode = 8001234567891;
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, *, 1.045 FROM Product WHERE Barcode = 5345738573637;
GO

INSERT INTO Bill DEFAULT VALUES;
DECLARE @lastBillNumber BIGINT;
SET @lastBillNumber = (SELECT MAX(Number) FROM Bill);
INSERT INTO Item (BillNumber, Barcode, [Name], Price, Quantity)
    SELECT @lastBillNumber, Barcode, [Name], Price, 1 FROM Product WHERE Barcode = 4634567890098;
GO