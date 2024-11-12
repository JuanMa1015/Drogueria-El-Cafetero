$('.add-cart-btn').click(function (e) {
    e.preventDefault();

    const form = $(this).closest("form");



    Swal.fire({
        position: "center",
        icon: "success",
        title: "Producto Añadido Al Carrito",
        showConfirmButton: false,
        timer: 1000
    }).then(() => {
        form.submit();
    });
});