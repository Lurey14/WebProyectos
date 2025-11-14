1. La arquitectura como tal es la arquitectura "Limpia" y "Por capas" ya que de esta manera acaba mas ordenada ya que lo ordenamos 
	por carpeta cada archivo y sabemos donde se encuentra cada una para que el proyecto como tal se vea de manera ordenada y entendible.

2. Como primer paso se creo todas las carpetas y los respectivos archivos como clase a excepcion del controller por el momento.

3. En Models se creo todas las entidades pedidas, con sus respectivos requerimientos para saber los atributos de cada uno:
	Producto: IdProducto, NombreProducto, DescripcionCorta, Precio, Stock, IdCategoria
	Categoria: IdCategoria, NombreCategoria, Descripcion
	Proveedor: IdProveedor, RazonSocial, Contacto

4. En DTOs se crea lo que es como tal todo los datos que sacamos de los modelos y los ponemos en las respectivas clases ya sea de Register, 
	Update, Delete segun lo que se necesite para cada uno.

5. Creamos el AppDbContext en la carpeta de Data para nuestra respectiva base de datos

6. Creamos el ProductoService en la carpeta de Services en el cual llamaremos a nuestras clases con nuestros respectivos datos, 
	las clases tal cual son las de Regsiter, Update, Delete para su funcionamiento

7. Creamos el ProductoController vacio para que podemos ahi mismo obtener los diferentes metodos como obtener, registrar, editar, 
	eliminar para su correcto funcionamiento

8. En Program añadimos lo que es basicamente nuestros services donde tenemos las funcionalidades y data donde esta nuestra base de datos

-Listar productos
-Crear productos
-Editar productos
-Eliminar productos
-Ordenar los productos por categoria
-Buscar productos por nombre
-Listar productos de un proveedor
-En el Readme.md debo explicar el porque de la arquitectura seleccionada