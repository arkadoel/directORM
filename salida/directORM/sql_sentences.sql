PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
/*
	Archivo generado por directORM
	Fecha: Sabado, 2015-01-31 19:09:02
*/

DROP TABLE IF EXISTS "Horarios";
CREATE TABLE "Horarios" (
    "idHorario" INTEGER PRIMARY KEY AUTOINCREMENT,
    "idEmpleado" INTEGER KEY,
    "dia_semana" VARCHAR,
    "hora_entrada" VARCHAR,
    "hora_salida" VARCHAR
);

DROP TABLE IF EXISTS "Productos_Proveedores";
CREATE TABLE "Productos_Proveedores" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "id_producto" INTEGER KEY,
    "id_proveedor" INTEGER KEY,
    "fecha" DATE,
    "cantidad" REAL
);

DROP TABLE IF EXISTS "Productos";
CREATE TABLE "Productos" (
    "idProducto" INTEGER PRIMARY KEY AUTOINCREMENT,
    "nombre_producto" VARCHAR,
    "precio_unidad" REAL,
    "iva" INTEGER,
    "foto_producto" VARCHAR,
    "ingredientes" VARCHAR,
    "familia" VARCHAR
);

DROP TABLE IF EXISTS "Empleados";
CREATE TABLE "Empleados" (
    "idEmpleado" INTEGER PRIMARY KEY AUTOINCREMENT,
    "nombre" VARCHAR,
    "apellidos" VARCHAR,
    "email" VARCHAR,
    "tlf_fijo" VARCHAR,
    "tlf_movil" VARCHAR,
    "direccion" VARCHAR,
    "foto_empleado" VARCHAR,
    "banco" VARCHAR,
    "cuenta_bancaria" VARCHAR,
    "sexo" VARCHAR,
    "password" VARCHAR,
    "login_name" VARCHAR,
    "cargo" VARCHAR
);

DROP TABLE IF EXISTS "Ordenes";
CREATE TABLE "Ordenes" (
    "idOrden" INTEGER PRIMARY KEY AUTOINCREMENT,
    "idEmpleado" INTEGER KEY,
    "num_ticket" VARCHAR,
    "fecha" DATE,
    "hora" DATE,
    "lugar" VARCHAR
);

DROP TABLE IF EXISTS "Proveedores";
CREATE TABLE "Proveedores" (
    "idProveedor" INTEGER PRIMARY KEY AUTOINCREMENT,
    "nombre" VARCHAR,
    "email" VARCHAR,
    "tlf_fijo" VARCHAR,
    "tlf_movil" VARCHAR,
    "tlf_fijo2" VARCHAR,
    "tlf_movil2" VARCHAR,
    "banco" VARCHAR,
    "cuenta_bancaria" VARCHAR,
    "direccion" VARCHAR,
    "foto_logo" VARCHAR
);

DROP TABLE IF EXISTS "Productos_en_Ordenes";
CREATE TABLE "Productos_en_Ordenes" (
    "idOrden" INTEGER KEY,
    "idProducto" INTEGER KEY,
    "cantidad" REAL
);


COMMIT;