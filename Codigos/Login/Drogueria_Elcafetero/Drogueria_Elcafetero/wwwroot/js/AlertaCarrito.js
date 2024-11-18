$('.add-cart-btn').click(function (e) {
    console.log("Botón clickeado"); // Para verificar si la función se está ejecutando
    e.preventDefault(); // Evitar el envío automático del formulario

    const form = $(this).closest("form"); // Obtener el formulario al que pertenece el botón
    const productoId = form.find("input[name='id_product']").val(); // Obtener el ID del producto desde el formulario
    const stockDisponible = parseInt($(this).attr('data-stock'));  // Obtener el stock disponible del producto desde el atributo 'data-stock'
    const stockCarrito = parseInt($('#quantity-' + productoId).val()) || 0; // Obtener la cantidad en el carrito (si existe)


    // Verificar si el producto está agotado
    if (stockDisponible <= 0) {
        Swal.fire({
            position: "center",
            icon: "error",
            title: "No Hay Unidades Disponibles",
            showConfirmButton: false,
            timer: 1500
        });
        return; // No enviar el formulario si el producto está agotado
    }

    // Mostrar el mensaje de éxito si hay stock disponible y no se ha alcanzado el límite en el carrito
    Swal.fire({
        position: "center",
        icon: "success",
        title: "Producto agregado al carrito",
        showConfirmButton: false,
        timer: 1500
    }).then(() => {
        form.submit(); // Enviar el formulario después de que la alerta desaparezca
    });
});