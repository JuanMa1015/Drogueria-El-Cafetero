-- Tipo ENUM para estados
CREATE TYPE order_state AS ENUM ('Pending', 'Completed', 'Cancelled');

-- Tabla Department
CREATE TABLE Department (
    department_name VARCHAR(15) PRIMARY KEY
);

-- Tabla City_towns
CREATE TABLE City_towns (
    city_name VARCHAR(20) PRIMARY KEY NOT NULL,
    department_name VARCHAR(15) REFERENCES Department(department_name)
);

-- Tabla Suppliers
CREATE TABLE Suppliers (
    id_supplier SERIAL PRIMARY KEY,
    supplier_name VARCHAR(50) NOT NULL,
    telephone VARCHAR(10),
    email VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Address
CREATE TABLE Address (
    id_address SERIAL PRIMARY KEY,
    city_name VARCHAR(20) REFERENCES City_towns(city_name),
    description VARCHAR(50) NOT NULL,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE
);

-- Tabla Products
CREATE TABLE Products (
    id_product SERIAL PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL,
    price DECIMAL CHECK (price >= 0),
    units_in_stock INT NOT NULL CHECK (units_in_stock >= 0),
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    expiration_date DATE NOT NULL CHECK (expiration_date >= CURRENT_DATE),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Índice en id_supplier para Products
CREATE INDEX idx_id_supplier_product ON Products(id_supplier);

-- Tabla Suppliers_products
CREATE TABLE Suppliers_products (
    id_supplier_product SERIAL PRIMARY KEY,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    price DECIMAL CHECK (price >= 0),
    agreement_date DATE DEFAULT CURRENT_DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Índice compuesto para Suppliers_products
CREATE INDEX idx_product_supplier ON Suppliers_products(id_product, id_supplier);

-- Tabla Purchase_orders
CREATE TABLE Purchase_orders (
    id_purchase_order SERIAL PRIMARY KEY,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    order_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_order DECIMAL NOT NULL CHECK (total_order >= 0),
    state order_state,  -- Cambiado a ENUM
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Suppliers_invoices
CREATE TABLE Suppliers_invoices (
    id_supplier_invoice SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    invoice_number VARCHAR(100) NOT NULL UNIQUE,  -- Unicidad añadida
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state order_state NOT NULL,  -- Cambiado a ENUM
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Discount
CREATE TABLE Discount (
    id_discount SERIAL PRIMARY KEY,
    id_supplier_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice) ON DELETE CASCADE,
    description VARCHAR(50) NOT NULL,
    discount_type VARCHAR(30) NOT NULL,
    discount_value DECIMAL CHECK (discount_value >= 0),
    start_date DATE NOT NULL DEFAULT CURRENT_DATE,
    end_date DATE NOT NULL CHECK (end_date > start_date),
    conditions VARCHAR(80) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Discount_Suppliers_invoices
CREATE TABLE Discount_Suppliers_invoices (
    id_discount_Suppliers_invoices SERIAL PRIMARY KEY,
    id_discount INT REFERENCES Discount(id_discount) ON DELETE CASCADE,
    id_supplier_invoice INT REFERENCES Suppliers_invoices(id_supplier_invoice) ON DELETE CASCADE,
    discount_amount DECIMAL NOT NULL CHECK (discount_amount >= 0),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Employees
CREATE TABLE Employees (
    id_employee SERIAL PRIMARY KEY,
    employee_name VARCHAR(50) NOT NULL,
    role VARCHAR(50) NOT NULL,
    salary DECIMAL NOT NULL CHECK (salary >= 0),
    hiring_date DATE NOT NULL DEFAULT CURRENT_DATE,
    email VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Customers
CREATE TABLE Customers (
    id_customer SERIAL PRIMARY KEY,
    customer_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    token VARCHAR(50) NOT NULL UNIQUE,
    confirmed BOOLEAN NOT NULL,
    reset_password BOOLEAN NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Sales
CREATE TABLE Sales (
    id_sale SERIAL PRIMARY KEY,
    id_customer INT REFERENCES Customers(id_customer) ON DELETE CASCADE,
    id_employee INT REFERENCES Employees(id_employee) ON DELETE CASCADE,
    sale_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_sale DECIMAL NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Sales_Details
CREATE TABLE Sales_Details (
    id_detail SERIAL PRIMARY KEY,
    id_sale INT REFERENCES Sales(id_sale) ON DELETE CASCADE,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    amount_products INT NOT NULL,
    unit_price DECIMAL NOT NULL CHECK (unit_price >= 0),
    subtotal DECIMAL NOT NULL CHECK (subtotal >= 0),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Sales_invoices
CREATE TABLE Sales_invoices (
    id_invoice SERIAL PRIMARY KEY,
    id_sale INT REFERENCES Sales(id_sale) ON DELETE CASCADE,
    issue_number VARCHAR(100) NOT NULL UNIQUE,  -- Unicidad añadida
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state order_state NOT NULL,  -- Cambiado a ENUM
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Purchase_Orders_Details
CREATE TABLE Purchase_Orders_Details (
    id_order_detail SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    id_product INT REFERENCES Products(id_product) ON DELETE CASCADE,
    amount_product INT NOT NULL,
    unit_price DECIMAL NOT NULL CHECK (unit_price >= 0),
    subtotal DECIMAL NOT NULL CHECK (subtotal >= 0),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabla Purchase_orders_invoice
CREATE TABLE Purchase_orders_invoice (
    id_purchase_invoice SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    invoice_number VARCHAR(100) NOT NULL UNIQUE,  -- Unicidad añadida
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state order_state NOT NULL,  -- Cambiado a ENUM
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Índice en city_name para Address
CREATE INDEX idx_city_name ON Address(city_name);

-- Índices adicionales para mejorar el rendimiento
CREATE INDEX idx_customer_name ON Customers(customer_name);
CREATE INDEX idx_employee_name ON Employees(employee_name);
