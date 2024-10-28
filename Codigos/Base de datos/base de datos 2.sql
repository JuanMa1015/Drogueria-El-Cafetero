-- Crear tablas base primero, sin dependencias
CREATE TABLE Department (
    department_name VARCHAR(15) PRIMARY KEY
);

CREATE TABLE Customers (
    id_customer SERIAL PRIMARY KEY,
    customer_name VARCHAR(50) NOT NULL
);

-- Crear tablas dependientes de las tablas base
CREATE TABLE City_towns (
    city_name VARCHAR(20) PRIMARY KEY NOT NULL,
    department_name VARCHAR(15) REFERENCES Department(department_name)
);

CREATE TABLE Suppliers (
    id_supplier SERIAL PRIMARY KEY,
    supplier_name VARCHAR(50) NOT NULL,
    telephone VARCHAR(10),
    email VARCHAR(50) NOT NULL UNIQUE
);

-- Crear tablas con dependencias adicionales
CREATE TABLE Address (
    id_address SERIAL PRIMARY KEY,
    city_name VARCHAR(20) REFERENCES City_towns(city_name),
    description VARCHAR(50) NOT NULL,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE
);

CREATE TABLE Products (
    id_product SERIAL PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL,
    price DECIMAL CHECK (price >= 0),
    units_in_stock INT NOT NULL CHECK (units_in_stock >= 0),
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    expiration_date DATE NOT NULL CHECK (expiration_date >= CURRENT_DATE)
);

-- Índices
CREATE INDEX idx_id_supplier_product ON Products(id_supplier);

-- Crear tabla intermedia para relaciones muchos a muchos
CREATE TABLE Suppliers_products (
    id_supplier_product SERIAL PRIMARY KEY,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    price DECIMAL CHECK (price >= 0),
    agreement_date DATE DEFAULT CURRENT_DATE
);

CREATE INDEX idx_product_supplier ON Suppliers_products(id_product, id_supplier);

-- Crear tabla de órdenes de compra y dependientes
CREATE TABLE Purchase_orders (
    id_purchase_order SERIAL PRIMARY KEY,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    order_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_order DECIMAL NOT NULL CHECK (total_order >= 0),
    state VARCHAR(30)
);

CREATE TABLE Suppliers_invoices (
    id_supplier_invoice SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    invoice_number VARCHAR(100) NOT NULL,
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state VARCHAR(30) NOT NULL
);

-- Tabla de descuentos y relación con facturas de proveedores
CREATE TABLE Discount (
    id_discount SERIAL PRIMARY KEY,
    id_supplier_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice) ON DELETE CASCADE,
    description VARCHAR(50) NOT NULL,
    discount_type VARCHAR(30) NOT NULL,
    discount_value DECIMAL CHECK (discount_value >= 0),
    start_date DATE NOT NULL DEFAULT CURRENT_DATE,
    end_date DATE NOT NULL CHECK (end_date > start_date),
    conditions VARCHAR(80) NOT NULL
);

CREATE TABLE Discount_Suppliers_invoices (
    id_discount_Suppliers_invoices SERIAL PRIMARY KEY,
    id_discount INT REFERENCES Discount(id_discount) ON DELETE CASCADE,
    id_supplier_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice) ON DELETE CASCADE,
    discount_amount DECIMAL NOT NULL CHECK (discount_amount >= 0)
);

-- Crear tablas para empleados y roles
CREATE TABLE Employees (
    id_employee SERIAL PRIMARY KEY,
    employee_name VARCHAR(50) NOT NULL,
    salary DECIMAL NOT NULL CHECK (salary >= 0),
    hiring_date DATE NOT NULL DEFAULT CURRENT_DATE,
    email VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(20) DEFAULT 'Empleado'
);

-- Crear tabla para usuarios
CREATE TABLE Users (
    id_user SERIAL PRIMARY KEY,
    user_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    token VARCHAR(50) NOT NULL UNIQUE,
    confirmed BOOLEAN NOT NULL,
    reset_password BOOLEAN NOT NULL,
    confirmed_password VARCHAR(255),
    rol VARCHAR(20) DEFAULT 'Cliente'
);

-- Crear tablas de ventas y detalles de ventas
CREATE TABLE Sales (
    id_sale SERIAL PRIMARY KEY,
    id_customer INT REFERENCES Customers(id_customer) ON DELETE CASCADE,
    id_employee INT REFERENCES Employees(id_employee) ON DELETE CASCADE,
    sale_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_sale DECIMAL NOT NULL
);

CREATE TABLE Sales_Details (
    id_detail SERIAL PRIMARY KEY,
    id_sale INT REFERENCES Sales(id_sale) ON DELETE CASCADE,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    amount_products INT NOT NULL,
    unit_price DECIMAL NOT NULL CHECK (unit_price >= 0),
    subtotal DECIMAL NOT NULL CHECK (subtotal >= 0)
);

CREATE TABLE Sales_invoices (
    id_invoice SERIAL PRIMARY KEY,
    id_sale INT REFERENCES Sales(id_sale) ON DELETE CASCADE,
    issue_number VARCHAR(100) NOT NULL,
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state VARCHAR(30) NOT NULL
);

-- Detalles de órdenes de compra y sus facturas
CREATE TABLE Purchase_Orders_Details (
    id_order_detail SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    amount_product INT NOT NULL,
    unit_price DECIMAL NOT NULL CHECK (unit_price >= 0),
    subtotal DECIMAL NOT NULL CHECK (subtotal >= 0)
);

CREATE TABLE Purchase_orders_invoice (
    id_purchase_invoice SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    invoice_number VARCHAR(100) NOT NULL,
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state VARCHAR(30) NOT NULL
);

-- Índice adicional
CREATE INDEX idx_city_name ON Address(city_name);
