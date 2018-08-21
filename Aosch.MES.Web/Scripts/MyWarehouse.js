
$(function () {
    FillWareHouseFrameFansjay("1");
    FillStoreRowFansjay("1");
});
function FillWareHouseFrameFansjay(warehouseno) {
    var storesPanelDiv = document.getElementById("storeContent");
    storesPanelDiv.innerHTML = '<div class="warehousewh" id="L88R1C1"></div><div class="warehousewh" id="L88R1C2"></div><div class="warehousewh" id="L88R1C3"></div><div class="warehousewh" id="L88R1C4"></div><div class="warehousewh" id="L88R1C5"></div><div class="warehousewh" id="L88R1C6"></div><div class="warehousewh" id="L88R1C7"></div><div class="warehousewh" id="L88R1C8"></div><div class="warehousewh" id="L88R1C9"></div><div class="warehousewh" id="L88R2C1"></div><div class="warehousewh" id="L88R2C2"></div><div class="warehousewh" id="L88R2C3"></div><div class="warehousewh" id="L88R2C4"></div><div class="warehousewh" id="L88R2C5"></div><div class="warehousewh" id="L88R2C6"></div><div class="warehousewh" id="L88R2C7"></div><div class="warehousewh" id="L88R2C8"></div><div class="warehousewh" id="L88R2C9"></div><div class="warehousewh" id="L88R3C1"></div><div class="warehousewh" id="L88R3C2"></div><div class="warehousewh" id="L88R3C3"></div><div class="warehousewh" id="L88R3C4"></div><div class="warehousewh" id="L88R3C5"></div><div class="warehousewh" id="L88R3C6"></div><div class="warehousewh" id="L88R3C7"></div><div class="warehousewh" id="L88R3C8"></div><div class="warehousewh" id="L88R3C9"></div><div class="warehousebwh" id="L88R4C1"></div><div class="warehousebwh" id="L88R4C2"></div><div class="warehousebwh" id="L88R4C3"></div><div class="warehousebwh" id="L88R4C4"></div><div class="warehousebwh" id="L88R4C5"></div><div class="warehousebwh" id="L88R4C6"></div><div class="warehousebwh" id="L88R5C1"></div><div class="warehousebwh" id="L88R5C2"></div><div class="warehousebwh" id="L88R5C3"></div><div class="warehousebwh" id="L88R5C4"></div><div class="warehousebwh" id="L88R5C5"></div><div class="warehousebwh" id="L88R5C6"></div><div class="warehousebwh" id="L88R6C1"></div><div class="warehousebwh" id="L88R6C2"></div><div class="warehousebwh" id="L88R6C3"></div><div class="warehousebwh" id="L88R6C4"></div><div class="warehousebwh" id="L88R6C5"></div><div class="warehousebwh" id="L88R6C6"></div>';  
}
function FillStoreRowFansjay(warehouseno) {
    $.getJSON("/WareHouse/GetWarehouseJsonFansjay?WarehouseNo="+warehouseno, function (data) {
        $.each(data, function (i, item) {
            var divName = item.PlaceName;
            var partplaceDiv = document.getElementById(divName);
            if (partplaceDiv != null) {
                var PartRFIDStr ="RFID:"+ item.PartRFID;
                $(partplaceDiv).attr("title", PartRFIDStr);
            }
            if (partplaceDiv != undefined && partplaceDiv != null) {//过滤W开头的DIV
                var lineNumber = parseInt(divName.substr(-3, 1));
                if (lineNumber > 3) {//后三行
                    partplaceDiv.style.backgroundImage = "url('../../Content/Images/rotatewarehouse/warehouse2/" + item.PartImageName + "')";
                } else {
                    partplaceDiv.style.backgroundImage = "url('../../Content/Images/rotatewarehouse/warehouse/" + item.PartImageName + "')";
                }
               
            }
        });
    });

}