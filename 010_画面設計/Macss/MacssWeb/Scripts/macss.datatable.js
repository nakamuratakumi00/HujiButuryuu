/// <reference path="typings/jquery/jquery.d.ts" />
var Macss;
(function (Macss) {
    var DataTables;
    (function (DataTables) {
        var MacssDataTable = /** @class */ (function () {
            function MacssDataTable(tableElementId, paramColumnDefs, leftColumnNum) {
                if (paramColumnDefs === void 0) { paramColumnDefs = [{
                        "className": "macss-table-proc-column", "width": 90, "orderable": false, "targets": 0
                    }]; }
                if (leftColumnNum === void 0) { leftColumnNum = 1; }
                this.id = tableElementId;
                if (paramColumnDefs == null) {
                    paramColumnDefs = [{
                            "className": "macss-table-proc-column", "width": 90, "orderable": false, "targets": 0
                        }];
                }
                this.tableOptions = {
                    lengthMenu: [5, 10, 20, 50, 100],
                    order: [],
                    fixedHeader: true,
                    pagingType: "simple_numbers",
                    scrollX: true,
                    scrollY: "360px",
                    scrollCollapse: true,
                    dom: "<'row macss-table-info-row'<'col-2 macss-table-display-count'l><'col-4'><'col-6 macss-paginate-button'p>>" +
                        "<tr>" +
                        "<'macss-table-info-row'<i><'macss-paginate-button'p>>",
                    columnDefs: paramColumnDefs,
                    //buttons: [
                    //    {
                    //        extend: 'colvis',
                    //        text: 'カラム切替'
                    //    },
                    //    {
                    //        extend: 'copy',
                    //        text: 'Copy to clipboard'
                    //    },
                    //    'pdf',
                    //    'excel',
                    //],
                    language: {
                        lengthMenu: "表示行数 _MENU_ 件",
                        paginate: {
                            next: "次へ",
                            previous: "前へ"
                        },
                        info: "全_TOTAL_件中 _START_件から_END_件を表示",
                        infoFiltered: ""
                    },
                    fixedColumns: {
                        leftColumns: leftColumnNum,
                    }
                };
            }
            MacssDataTable.prototype.getDatatable = function () {
                return this.dataTable;
            };
            MacssDataTable.prototype.configureDefault = function () {
                var tbl = $(this.id).DataTable();
                this.dataTable = tbl;
            };
            MacssDataTable.prototype.setOption = function (key, value) {
                this.tableOptions[key] = value;
            };
            MacssDataTable.prototype.setLengthMenu = function (paramLengthMenu) {
                this.setOption("lengthMenu", paramLengthMenu);
            };
            MacssDataTable.prototype.setScrollY = function (paramScrollY) {
                this.setOption("scrollY", paramScrollY);
            };
            MacssDataTable.prototype.configure = function () {
                var tbl = $(this.id).DataTable(this.tableOptions);
                this.dataTable = tbl;
            };
            MacssDataTable.prototype.configureWithOptions = function (options) {
                if (options === void 0) { options = {}; }
                var tbl = $(this.id).DataTable(options);
                this.dataTable = tbl;
            };
            return MacssDataTable;
        }());
        DataTables.MacssDataTable = MacssDataTable;
        var MacssSearchDialogTable = /** @class */ (function () {
            function MacssSearchDialogTable(tableElementId, paramColumnDefs) {
                if (paramColumnDefs === void 0) { paramColumnDefs = []; }
                this.id = tableElementId;
                this.doneConfig = false;
                this.tableOptions = {
                    lengthMenu: [10, 20, 50, 100],
                    searching: false,
                    order: [],
                    fixedHeader: true,
                    pagingType: "simple_numbers",
                    scrollX: true,
                    scrollY: "300px",
                    scrollCollapse: true,
                    dom: "<'row macss-table-info-row'<'col-2 macss-table-display-count'l><'col-4'><'col-6 macss-paginate-button'p>>" +
                        "<tr>" +
                        "<'macss-table-info-row'<i><'macss-paginate-button'p>>",
                    columnDefs: paramColumnDefs,
                    language: {
                        lengthMenu: "表示行数 _MENU_ 件",
                        paginate: {
                            next: "次へ",
                            previous: "前へ"
                        },
                        info: "全_TOTAL_件中 _START_件から_END_件を表示"
                    }
                };
            }
            MacssSearchDialogTable.prototype.configure = function (paramScrollY) {
                if (paramScrollY === void 0) { paramScrollY = "300px"; }
                if (this.doneConfig) {
                    return;
                }
                this.tableOptions["scrollY"] = paramScrollY;
                this.dataTable = $(this.id).DataTable(this.tableOptions);
                this.doneConfig = true;
            };
            MacssSearchDialogTable.prototype.init = function () {
                if (this.doneConfig) {
                    this.dataTable.state.clear();
                    this.dataTable.destroy();
                }
                $(this.id + " tbody").empty();
            };
            MacssSearchDialogTable.prototype.refresh = function () {
                this.dataTable = $(this.id).DataTable(this.tableOptions);
                this.doneConfig = true;
            };
            return MacssSearchDialogTable;
        }());
        DataTables.MacssSearchDialogTable = MacssSearchDialogTable;
        ///
        /// リストのみ（ページャーなどがありません）
        ///
        var MacssSimpleTable = /** @class */ (function () {
            function MacssSimpleTable(tableElementId, order) {
                if (order === void 0) { order = false; }
                this.id = tableElementId;
                this.tableOptions = {
                    ordering: order,
                    fixedHeader: false,
                    paging: false,
                    scrollX: true,
                    scrollY: "360px",
                    scrollCollapse: true,
                    dom: "<'col-4'>"
                };
            }
            MacssSimpleTable.prototype.getDatatable = function () {
                return this.dataTable;
            };
            MacssSimpleTable.prototype.configureDefault = function () {
                var tbl = $(this.id).DataTable();
                this.dataTable = tbl;
            };
            MacssSimpleTable.prototype.setOption = function (key, value) {
                this.tableOptions[key] = value;
            };
            MacssSimpleTable.prototype.setScrollY = function (paramScrollY) {
                this.setOption("scrollY", paramScrollY);
            };
            MacssSimpleTable.prototype.configure = function () {
                var tbl = $(this.id).DataTable(this.tableOptions);
                this.dataTable = tbl;
            };
            MacssSimpleTable.prototype.configureWithOptions = function (options) {
                if (options === void 0) { options = {}; }
                var tbl = $(this.id).DataTable(options);
                this.dataTable = tbl;
            };
            return MacssSimpleTable;
        }());
        DataTables.MacssSimpleTable = MacssSimpleTable;
    })(DataTables = Macss.DataTables || (Macss.DataTables = {}));
})(Macss || (Macss = {}));
//# sourceMappingURL=macss.datatable.js.map