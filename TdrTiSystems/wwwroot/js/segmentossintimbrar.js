window.onload = function () {
    listarSegmentos();
}


var objSegmentos;
function listarSegmentos() {
    objSegmentos = {
        url: "SegmentosSinTimbrar/listarSegmentos",
        cabeceras: ["Segmento","Status del Folio","Cliente","Fecha Creación","Fecha de Timbrado"],
        propiedades: ["segmento","folio","cliente","fechac","fechat"],
        paginar: true
    }
    pintar(objSegmentos, null)
}

function Buscar() {
    var segmento = get("txtsegmento")
    var cliente = get("txtcliente")
    objSegmentos.url = "SegmentosSinTimbrar/filtrarCpS/?segmento=" + segmento + "&cliente=" + cliente
    pintar(objSegmentos)
}

function Limpiar() {
    listarSegmentos();
    set("txtsegmento", "");
    set("txtcliente", "");
}
/* con esto limpiamos el formulario d
function LimpiarNombres() {
LimpiarDatos("frmBusqueda");
    setN("nombre","")
    listarCartaPorte()
}
*/