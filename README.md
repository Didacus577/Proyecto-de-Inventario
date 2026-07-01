## ⚙️ Funciones del Sistema

El sistema cuenta con un control de acceso basado en roles . A continuación, se detallan las responsabilidades y módulos asignados a cada perfil:

### 👤 Administrador
Tiene acceso total al sistema y está a cargo de las configuraciones maestras:
* **Gestión de Usuarios:** Registro, edición y control de las cuentas que acceden al sistema.
* **Gestión de Roles:** Asignación y control de permisos dentro de la aplicación.
* **Gestión de Categorías:** Clasificación de los productos para un catálogo organizado.
* **Gestión de Unidades de Medida:** Configuración de las métricas de los productos (unidades, kilogramos, litros, etc.).
* **Gestión de Marcas:** Control de los fabricantes o marcas de la mercancía.
* **Gestión de Proveedores:** Administración de los datos de contacto y empresas que surten el supermercado.
* **Gestión de Productos:** Catálogo maestro de artículos (stock, descripciones).
* **Gestión de Movimientos:** Control global y auditoría de entradas y salidas.

### 👥 Operador
Perfil enfocado en la operación diaria del inventario:
* **Gestión de Productos:** registro y edicion.
* **Gestión de Movimientos:** Registro y control operativo de las entradas y salidas de mercancía.

---

## 🛠️ Arquitectura del Proyecto

El proyecto está estructurado de la siguiente manera, respetando las responsabilidades de cada capa:

* **`Inventario.AppWeb`**: Capa de presentación (Interfaz de usuario). Contiene las vistas, controladores y recursos web principales.
* **`Inventario.IOC` (Inversión de Control)**: Capa encargada de la inyección de dependencias, vinculando la capa de datos y negocio con la interfaz de usuario.
* **`Inventario.BLL` (Business Logic Layer)**: Capa de lógica de negocio. Contiene las reglas, validaciones y procesos principales del sistema.
* **`Inventario.DAL` (Data Access Layer)**: Capa de acceso a datos. Maneja la comunicación directa con la base de datos (Contexto, Repositorios).
* **`Inventario.DTOS`**: Objetos de Transferencia de Datos utilizados para mover información limpia entre capas sin exponer las entidades mapeadas.
* **`Inventario.Entity`**: Modelos de dominio y entidades que representan las tablas de la base de datos.

---

## 🚀 Tecnologías Utilizadas

* **Backend:** .NET (C#)
* **Seguridad:** JWT (JSON Web Tokens) para autenticación y autorización segura basada en roles.
* **Acceso a Datos:** Entity Framework Core
* **Base de Datos:** SQL Server
* **Frontend:** Se utilizó MVC con Bootstrap

---

## 🔒 Seguridad con JWT

El sistema implementa autenticación mediante tokens **JWT**, asegurando que los endpoints de negocio y los controladores estén protegidos. 

* **Generación de Token:** Al iniciar sesión correctamente, el sistema emite un token firmado.
* **Autorización:** Las peticiones subsiguientes deben incluir el token en el encabezado de autorización para acceder a los recursos protegidos según el rol del usuario.
