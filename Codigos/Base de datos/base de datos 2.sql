-- Crear tablas base primero, sin dependencias
CREATE TABLE Auditory (
id_auditory SERIAL PRIMARY KEY NOT NULL,
table_name VARCHAR(30) NOT NULL,
action VARCHAR(10) NOT NULL,
previous_data TEXT,
new_data TEXT,
action_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
usser VARCHAR(20) NOT NULL
);

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

CREATE TABLE suppliers_invoices (
    id_supplier_invoice SERIAL PRIMARY KEY,
    id_purchase_order INT REFERENCES Purchase_orders(id_purchase_order) ON DELETE CASCADE,
    invoice_number VARCHAR(100) NOT NULL,
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_invoice DECIMAL NOT NULL CHECK (total_invoice >= 0),
    state BOOLEAN
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

CREATE TABLE Customers (
    id
)

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
    state BOOLEAN
);

-- Crear tabla de órdenes de compra y dependientes
CREATE TABLE Purchase_orders (
    id_purchase_order SERIAL PRIMARY KEY,
    id_supplier INT REFERENCES Suppliers(id_supplier) ON DELETE CASCADE,
    order_date DATE NOT NULL DEFAULT CURRENT_DATE,
    total_order DECIMAL NOT NULL CHECK (total_order >= 0),
    state BOOLEAN
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
    state BOOLEAN
);



-- Índice adicional
CREATE INDEX idx_city_name ON Address(city_name);

CREATE TABLE car (
    id_car SERIAL PRIMARY KEY,
    id_user INT references users(id_user) ON delete cascade,
    id_product INT references products(id_product) ON delete cascade,
    quantity INT NOT NULL,
    date TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
    total_price INTEGER DEFAULT 0
);


--Funcion para la auditoría
CREATE OR REPLACE FUNCTION fn_accion_auditory() 
RETURNS TRIGGER AS
$$
BEGIN 
	IF (TG_OP = 'INSERT') THEN 
		INSERT INTO Auditory(table_name, action, previous_data, new_data, action_date, usser)
		VALUES (TG_TABLE_NAME, 'INSERT', NULL, NEW, NOW(), USER);
		RETURN NEW;
	ELSIF (TG_OP = 'UPDATE') THEN
		INSERT INTO Auditory(table_name, action, previous_data, new_data, action_date, usser)
		VALUES (TG_TABLE_NAME, 'UPDATE', OLD, NEW, NOW(), USER);
		RETURN NEW;
	ELSIF (TG_OP = 'DELETE') THEN
		INSERT INTO Auditory(table_name, action, previous_data, new_data, action_date, usser)
		VALUES (TG_TABLE_NAME, 'DELETE', OLD, NULL, NOW(), USER);
		RETURN OLD;
	END IF;
	RETURN NULL;
END;
$$
LANGUAGE plpgsql;

--Triggers para la auditoria

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Department FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Customers FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON City_towns FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Suppliers FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Address FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Products FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON category FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON suppliers_invoices FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Discount FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Discount_Suppliers_invoices FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Employees FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Users FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Sales FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Sales_Details FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

/* CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Sales_invoices FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()*/

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Purchase_orders FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Purchase_Orders_Details FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()

CREATE TRIGGER takes_trigger_auditoria AFTER INSERT OR UPDATE OR DELETE
ON Purchase_orders_invoice FOR EACH ROW EXECUTE PROCEDURE fn_accion_auditory()



CREATE OR REPLACE PROCEDURE agregar_al_carrito(
    p_id_producto INT,
    p_id_usuario INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_stock INT;
    v_existe_carrito INT;
    v_precio DECIMAL;
BEGIN
    -- Obtener el stock disponible del producto
    SELECT units_in_stock INTO v_stock
    FROM products
    WHERE id_product = p_id_producto;

    -- Verificar si el producto tiene stock
    IF v_stock = 0 THEN
        RAISE EXCEPTION 'Producto sin stock disponible';
    END IF;

    -- Verificar si el producto ya está en el carrito
    SELECT COUNT(*) INTO v_existe_carrito
    FROM car
    WHERE id_user = p_id_usuario AND id_product = p_id_producto;

    IF v_existe_carrito > 0 THEN
        -- Si el producto ya está en el carrito, incrementar la cantidad
        UPDATE car
        SET quantity = quantity + 1
        WHERE id_user = p_id_usuario AND id_product = p_id_producto;
    ELSE
        -- Si el producto no está en el carrito, agregarlo
        SELECT price INTO v_precio
        FROM products
        WHERE id_product = p_id_producto;

        INSERT INTO car (id_user, id_product, quantity, date, price)
        VALUES (p_id_usuario, p_id_producto, 1, CURRENT_TIMESTAMP, v_precio);
    END IF;

    -- Actualizar el stock del producto
    UPDATE products
    SET units_in_stock = units_in_stock - 1
    WHERE id_product = p_id_producto;

    COMMIT;
END;
$$;

CREATE OR REPLACE PROCEDURE eliminar_del_carrito(
    p_id_car INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_id_producto INT;
    v_cantidad INT;
BEGIN
    -- Obtener el id del producto y la cantidad del carrito
    SELECT id_product, quantity INTO v_id_producto, v_cantidad
    FROM car
    WHERE id_car = p_id_car;

    -- Eliminar el producto del carrito
    DELETE FROM car
    WHERE id_car = p_id_car;

    -- Actualizar el stock del producto
    UPDATE products
    SET units_in_stock = units_in_stock + v_cantidad
    WHERE id_product = v_id_producto;

    COMMIT;
END;
$$;

CREATE OR REPLACE PROCEDURE obtener_carrito_usuario(
    p_id_usuario INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Obtener todos los productos en el carrito del usuario
    SELECT
        c.id_car,
        p.product_name AS nombre_producto,
        c.quantity AS cantidad,
        c.price AS precio,
        (c.quantity * c.price) AS total
    FROM car c
    JOIN products p ON c.id_product = p.id_product
    WHERE c.id_user = p_id_usuario;
END;
$$;

CREATE OR REPLACE FUNCTION actualizar_cantidad_producto(carrito_id INT, nueva_cantidad INT)
RETURNS NUMERIC AS
$$
DECLARE
    nuevo_precio NUMERIC := 0; -- Inicializa nuevo_precio en 0 para evitar NULL
    cantidad_actual INT;
    producto_id INT;
    diferencia INT;
BEGIN
    -- Selecciona la cantidad actual y el id del producto del carrito
    SELECT quantity, p.id_product
    INTO cantidad_actual, producto_id
    FROM car c
    JOIN products p ON c.id_product = p.id_product
    WHERE c.id_car = carrito_id;

    -- Verifica si se encontró un producto
    IF producto_id IS NULL THEN
        RAISE EXCEPTION 'Producto no encontrado para el carrito ID %', carrito_id;
    END IF;

    -- Calcula la diferencia entre la nueva cantidad y la cantidad actual
    diferencia := nueva_cantidad - cantidad_actual;

    -- Actualiza la cantidad en el carrito
    UPDATE car
    SET quantity = nueva_cantidad
    WHERE id_car = carrito_id;

    -- Actualiza el stock de productos según la diferencia calculada
    UPDATE products
    SET units_in_stock = units_in_stock - diferencia
    WHERE id_product = producto_id;

    -- Calcula el nuevo precio total para el producto en el carrito
    SELECT COALESCE(p.price * nueva_cantidad, 0) -- Usa COALESCE para evitar NULL
    INTO nuevo_precio
    FROM products p
    WHERE p.id_product = producto_id;

    RETURN nuevo_precio;
END;
$$ LANGUAGE plpgsql;
