window.onload = function () {
    listarSucursal();
}
var objSucursal;
function listarSucursal() {
    objSucursal = {
        url: "Sucursal/listarSucursal",
        cabeceras: ["Id Sucursal", "Nombre", "Direccion"],
        propiedades: ["iidsucursal", "nombre", "direccion"]
    }
    pintar(objSucursal)
}

function BuscarSucursal() {
    var nombresucursal = get("txtnombrebusqueda")
    objSucursal.url = "Sucursal/filtrarSucursal/?nombresucursal=" + nombresucursal
    pintar(objSucursal)
}
function LimpiarListaSucursal() {
    listarSucursal();
    set("txtnombrebusqueda", "")
    // document.getElementById("txtnombrebusqueda").value = "";
}

function GuardarDatos() {

    var errores = ValidarDatos("frmSucursal")
    if (errores != "") {
        Error(errores)
        return;
    }
    var frmGuardar = document.getElementById("frmSucursal");
    var frm = new FormData(frmGuardar);
    Confirmacion(undefined, undefined, function (rpta) {
        fetchPost("Sucursal/GuardarDatos", "text", frm, function (data) {
            if (data == "1") {
                Exito();
                listarSucursal();
               // LimpiarDatos("frmSucursal")
            } else Error();
        })
    })

}