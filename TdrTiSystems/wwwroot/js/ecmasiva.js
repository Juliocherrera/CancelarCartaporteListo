window.onload = function () {
    LHistorial();
}
var objMasiva;
function listarMasiva() {
    objMasiva = {
        url: "ECMasiva/listarMasiva",
        cabeceras: ["#Orden","Producto SAT","Descripción SAT","Piezas SAT","Count Units","Peso SAT","Weight Units"],
        propiedades: ["ai_orden", "av_cmd_code", "av_cmd_description", "af_count", "av_countunit", "af_weight", "av_weightunit"],
        paginar: true
    }
    pintar(objMasiva)
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

function Erroress(titulo = "Error", texto = "Ocurrio un error") {
    Swal.fire({
        icon: 'error',
        title: 'La orden ya fue procesada',
        text: 'Ingresa otra orden'
    })
}

function Pcon() {
    fetchGet("ECMasiva/cargarEnSQL", "json", function (data) {
        if (data == 1) {
            alert("1");
        } else {
            alert("Nada");
        }
    });
}

function LHistorial() {
    fetchGet("ECMasiva/limpiarHistorial", "json", function (data) {
        
    });
}

function RepeDatos() {
    LHistorial();
    var errores = ValidarDatos("frmMasiva")
    if (errores != "") {
        Error(errores)
        return;
    }
    var frmGuardar = document.getElementById("frmMasiva");
    var frm = new FormData(frmGuardar);

    fetchPost("ECMasiva/RepeDatos", "text", frm, function (data) {
        if (data != null) {
            fetchGet("ECMasiva/cargarEnSQL", "json", function (data) {
                if (data == 1) {
                    Erroress();
                } else {
                   
                    fetchGet("ECMasiva/cargarEnSQL2", "json", function (data) {
                        if (data == 1) {
                            Erroress();

                        } else {
                            ExitoM();
                            listarMasiva();
                            LimpiarDatos("frmMasiva")
                            
                        }
                    });
                }
            });
           
        }
        else Error();
    })
}

function GuardarDatos() {

    var errores = ValidarDatos("frmMasiva")
    if (errores != "") {
        Error(errores)
        return;
    }
    var frmGuardar = document.getElementById("frmMasiva");
    var frm = new FormData(frmGuardar);
    
    fetchPost("ECMasiva/GuardarDatos", "text", frm, function (data) {
        if (data != null) {
            ExitoM();
            listarMasiva();
            LimpiarDatos("frmMasiva")

        }
        else {
            Error();
        }
    });
}