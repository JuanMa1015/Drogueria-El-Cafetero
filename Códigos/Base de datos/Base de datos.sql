CREATE TABLE Suppliers(
id_supplier SERIAL PRIMARY KEY,
supplier_name VARCHAR(50) NOT NULL,
telephone VARCHAR(10),
email VARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Address(
id_address INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
city_name VARCHAR(20) REFERENCES City_towns(city_name),
department_name VARCHAR(15) REFERENCES Department(department_name),
description VARCHAR(50) NOT NULL,
id_supplier INT REFERENCES Suppliers(id_supplier)
)

CREATE TABLE City_towns(
city_name VARCHAR(20) PRIMARY KEY NOT NULL,
department_name VARCHAR(15) REFERENCES Department(department_name)
)

CREATE TABLE Department(
department_name VARCHAR(15) PRIMARY KEY
)

CREATE TABLE Suppliers_products(
id_supplier_product INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_product INT REFERENCES Products(id_product),
id_supplier INT REFERENCES Suppliers(id_supplier),
price DECIMAL,
agreement_date DATE
)

CREATE TABLE Suppliers_invoices(
id_supplier_invoice INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order),
invoice_number VARCHAR(100) NOT NULL,
issue_date DATE NOT NULL,
total_invoice Decimal NOT NULL,
state VARCHAR(30) NOT NULL
) 

CREATE TABLE Discount_Suppliers_invoices(
id_discount_Suppliers_invoices INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_discount INT REFERENCES Discount(id_discount),
id_suppliers_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice),
discount_amount Decimal NOT NULL
)

CREATE TABLE Discount(
id_discount INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_supplier_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice),
descrption VARCHAR(50) NOT NULL,
discount_type VARCHAR(30) NOT NULL,
discount_value Decimal NOT NULL,
start_date Date NOT NULL,
end_date Date NOT NULL,
Conditions VARCHAR(80) NOT NULL
)

CREATE TABLE Products(
id_product INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
product_name VARCHAR(50) NOT NULL,
price DECIMAL NOT NULL,
units_in_stock INT,
id_supplier INT REFERENCES Suppliers(id_supplier),
expiration_date DATE
)
ALTER TABLE Products
ALTER COLUMN expiration_date SET NOT NULL;
ALTER TABLE Products
ALTER COLUMN units_in_stock SET NOT NULL;

CREATE TABLE Employees(
id_employee INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
employee_name VARCHAR(50) NOT NULL,
rol VARCHAR(50) NOT NULL,
salary Decimal NOT NULL,
hiring_date DATE
)
ALTER TABLE Employees
ALTER COLUMN hiring_date SET NOT NULL;

CREATE TABLE Login_Employees(
id_login INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_employee INT REFERENCES Employees(id_employee),
username VARCHAR(30) NOT NULL,
password_hash VARCHAR(30) NOT NULL,
rol VARCHAR(20) NOT NULL,
last_access DATE NOT NULL
)

CREATE TABLE Customers(
id_customer INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
customer_name VARCHAR(50) NOT NULL,
telephone VARCHAR(10) NOT NULL,
email VARCHAR(50) NOT NULL
)

CREATE TABLE Login_customers(
id_login INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_customer INT REFERENCES Customers(id_customer),
username VARCHAR(30) NOT NULL,
password_hash VARCHAR(30) NOT NULL,
last_access DATE NOT NULL
)

CREATE TABLE Sales(
id_sale INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_customer INT REFERENCES Customers(id_customer),
id_employee INT REFERENCES Employees(id_employee),
sale_date DATE NOT NULL,
total_sale Decimal NOT NULL
)

CREATE TABLE Sales_Details(
id_detail INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_sale INT REFERENCES Sales(id_sale),
id_product INT REFERENCES Products(id_product),
amount_products INT NOT NULL,
unit_price Decimal NOT NULL,
subtotal Decimal NOT NULL
)

CREATE TABLE Sales_invoices(
id_invoice INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_sale INT REFERENCES Sales(id_sale),
issue_number VARCHAR(100) NOT NULL,
issue_date DATE NOT NULL,
total_invoice Decimal NOT NULL,
state VARCHAR(30) NOT NULL
)

CREATE TABLE Purchase_orders(
id_purchase_order INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_supplier INT REFERENCES Suppliers(id_supplier),
order_date DATE NOT NULL,
total_order Decimal NOT NULL,
state VARCHAR(30)
)

CREATE TABLE Purchase_Orders_Details(
id_order_detail INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order),
id_product INT REFERENCES Products(id_product),
amount_product INT NOT NULL,
unit_price Decimal NOT NULL,
subtotal Decimal NOT NULL
)

CREATE TABLE Purchase_orders_invoice(
id_purchase_invoice INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order),
invoice_number VARCHAR(100) NOT NULL,
issue_date DATE NOT NULL,
total_invoice Decimal NOT NULL,
state VARCHAR(30) NOT NULL
)



