window.onload = function () {
    listarCartaPorte();
}


var objCartaporte;
function listarCartaPorte() {
    objCartaporte = {
        url: "CartaPorte/listarCartaPorte",
        cabeceras: ["Folio", "Fecha", "Cliente", "Serie", "UUID", "ZIP", "PDF", "XML"],
        propiedades: ["folio", "fecha", "ord_billto", "serie", "uuid", "pdf_xml_descarga", "pdf_descargafactura", "xlm_descargafactura"],
        paginar: true
    }
    pintar(objCartaporte, null)
}

function Buscar() {
    var segmento = get("txtsegmento")
    objCartaporte.url = "CartaPorte/filtrarCartaPorte/?segmento=" + segmento
    pintar(objCartaporte)
}

function Limpiar() {
    listarCartaPorte();
    set("txtsegmento", "");
}
/* con esto limpiamos el formulario d
function LimpiarNombres() {
LimpiarDatos("frmBusqueda");
    setN("nombre","")
    listarCartaPorte()
}
*/