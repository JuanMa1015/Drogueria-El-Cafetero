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
    email VARCHAR(50) NOT NULL UNIQUE,
    active BOOL DEFAULT TRUE
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
    expiration_date DATE NOT NULL CHECK (expiration_date >= CURRENT_DATE),
    active BOOL DEFAULT TRUE,
    image VARCHAR(100),
    id_category INT REFERENCES category(id_category)
);

CREATE TABLE category (
    id_category INT SERIAL PRIMARY KEY,
    category_name VARCHAR(50),
    description VARCHAR(100),
    active BOOL DEFAULT TRUE    
)

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

CREATE TABLE car(
id_car serial primary key,
id_user int references users(id_user),
id_product int references products(id_product)
)

CREATE OR REPLACE FUNCTION sp_obtenerCategoria()
RETURNS SETOF category AS $$
BEGIN
    RETURN QUERY SELECT * FROM category;
END;
$$ LANGUAGE plpgsql;


SELECT * FROM sp_obtenerCategoria();

CREATE OR REPLACE FUNCTION sp_registrar_categoria(descripcion VARCHAR(50))
RETURNS BOOLEAN AS $$
DECLARE
    resultado BOOLEAN := TRUE;
BEGIN
    -- Verifica si la descripción ya existe en la tabla CATEGORIA
    IF EXISTS (SELECT 1 FROM categoria WHERE descripcion = descripcion) THEN
        resultado := FALSE;
    ELSE
        -- Inserta la nueva descripción en la tabla CATEGORIA
        INSERT INTO categoria(descripcion) VALUES (descripcion);
    END IF;

    RETURN resultado;
END;
$$ LANGUAGE plpgsql;

SELECT sp_registrar_categoria('TuDescripcion');

CREATE OR REPLACE FUNCTION sp_modificar_categoria(id_categoria INT, descripcion VARCHAR(60), activo BOOLEAN)
RETURNS BOOLEAN AS $$
DECLARE
    resultado BOOLEAN := TRUE;
BEGIN
    -- Verificar si existe otra categoría con la misma descripción y diferente IdCategoria
    IF EXISTS (SELECT 1 FROM categoria WHERE descripcion = descripcion AND idcategoria != id_categoria) THEN
        resultado := FALSE;
    ELSE
        -- Actualizar la categoría
        UPDATE categoria
        SET descripcion = descripcion,
            activo = activo
        WHERE idcategoria = id_categoria;
    END IF;

    RETURN resultado;
END;
$$ LANGUAGE plpgsql;

SELECT sp_modificar_categoria(1, 'NuevaDescripcion', TRUE);

CREATE OR REPLACE FUNCTION sp_obtener_proveedor()
RETURNS SETOF suppliers AS $$
BEGIN
    RETURN QUERY SELECT * FROM suppliers;
END;
$$ LANGUAGE plpgsql;
SELECT * FROM sp_obtener_proveedor();

CREATE OR REPLACE FUNCTION sp_registrar_proveedor(suppliername VARCHAR(50))
RETURNS BOOLEAN AS $$
DECLARE
    resultado BOOLEAN := TRUE;
BEGIN
    -- Verificar si la descripción ya existe en la tabla MARCA
    IF EXISTS (SELECT 1 FROM suppliers WHERE suppliername = supplier_name) THEN
        resultado := FALSE;
    ELSE
        -- Insertar la nueva descripción en la tabla MARCA
        INSERT INTO suppliers(supplier_name) VALUES (supplier_name);
    END IF;

    RETURN resultado;
END;
$$ LANGUAGE plpgsql;
SELECT sp_registrar_proveedor('TuDescripcion');

CREATE OR REPLACE FUNCTION sp_modificar_proveedor(idsupplier INT, suppliername VARCHAR(60), active BOOLEAN)
RETURNS BOOLEAN AS $$
DECLARE
    resultado BOOLEAN := TRUE;
BEGIN
    -- Verificar si existe otra marca con la misma descripción y diferente IdMarca
    IF EXISTS (SELECT 1 FROM suppliers WHERE suppliername = supplier_name AND idsupplier != id_supplier) THEN
        resultado := FALSE;
    ELSE
        -- Actualizar la marca
        UPDATE suppliers
        SET suppliername = supplier_name,
            active = active
        WHERE idsupplier = id_supplier;
    END IF;

    RETURN resultado;
END;
$$ LANGUAGE plpgsql;

SELECT sp_modificar_proveedor(1, 'NuevaDescripcion', TRUE);

CREATE OR REPLACE FUNCTION sp_obtener_producto()
RETURNS TABLE(idproduct INT, description VARCHAR, idsupplier INT, idcategory INT, price NUMERIC, active BOOLEAN, suppliername VARCHAR, description_category VARCHAR) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.*,
        s.description AS suppliername,
        c.description AS description_category
    FROM 
        producto p
    INNER JOIN 
        suppliers s ON s.id_supplier = p.id_supplier
    INNER JOIN 
        category c ON c.id_category = p.id_category;
END;
$$ LANGUAGE plpgsql;
SELECT * FROM sp_obtener_producto();

CREATE OR REPLACE FUNCTION sp_registrar_producto(
    product_name VARCHAR(50),
    price numeric,
    units_in_stock INT,
    id_supplier INT,
    expiration_date date,
    id_category INT,
    image VARCHAR(50),
    active bool 
)
RETURNS INT AS $$
DECLARE
    resultado INT := 0;
BEGIN
    -- Verificar si la descripción ya existe en la tabla PRODUCTO
    IF NOT EXISTS (SELECT 1 FROM product WHERE description = description) THEN
        -- Insertar el nuevo producto
        INSERT INTO product(product_name, price, units_in_stock, id_supplier, expiration_date, id_category, image, active)
        VALUES (product_name, price, units_in_stock, id_supplier, expiration_date, id_category, image, active)
        RETURNING id_product INTO resultado;  -- Obtener el ID del producto insertado
    END IF;

    RETURN resultado;  -- Retornar el ID del producto insertado o 0 si no se insertó
END;
$$ LANGUAGE plpgsql;
SELECT sp_registrar_producto('Producto Nombre', 'Descripción del Producto', 1, 2, 100.00, 50, '/ruta/a/la/imagen');

CREATE OR REPLACE FUNCTION sp_actualizar_ruta_imagen(
    idproduct INT,
    image VARCHAR(50)
)
RETURNS VOID AS $$
BEGIN
    -- Actualizar la ruta de la imagen para el producto especificado
    UPDATE product
    SET image = image
    WHERE idproduct = id_product;
END;
$$ LANGUAGE plpgsql;
SELECT sp_actualizar_ruta_imagen(1, '/nueva/ruta/a/la/imagen.jpg');

CREATE OR REPLACE FUNCTION sp_insertar_carrito(
    iduser INT,
    idproduct INT
)
RETURNS INT AS $$
DECLARE
    resultado INT := 0;
BEGIN
    -- Verificar si el producto ya está en el carrito para el usuario
    IF NOT EXISTS (SELECT 1 FROM car WHERE idproduct = id_product AND iduser = id_user) THEN
        -- Actualizar el stock del producto
        UPDATE product
        SET units_in_stock = units_in_stock - 1
        WHERE idproduct = id_product;

        -- Insertar el producto en el carrito
        INSERT INTO car(id_user, id_product)
        VALUES (iduser, idproduct);

        -- Asignar el valor 1 a resultado indicando éxito
        resultado := 1;
    END IF;

    -- Retornar el resultado (1 si fue exitoso, 0 si no se insertó)
    RETURN resultado;
END;
$$ LANGUAGE plpgsql;
SELECT sp_insertar_carrito(1, 101);

CREATE OR REPLACE FUNCTION sp_obtener_carrito(iduser INT)
RETURNS TABLE(
    idcar INT,
    idproduct INT,
    suppliername VARCHAR,
    productname VARCHAR,
    price NUMERIC,
    image VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.idcar, 
        p.idproduct, 
        s.suppliername AS suppliername, 
        p.productname, 
        p.price, 
        p.image
    FROM car c
    INNER JOIN product p ON p.idproduct = c.idproduct
    INNER JOIN suppliers s ON s.idsupplier = p.idsupplier
    WHERE c.iduser = iduser;
END;
$$ LANGUAGE plpgsql;
SELECT * FROM sp_obtener_carrito(1);




