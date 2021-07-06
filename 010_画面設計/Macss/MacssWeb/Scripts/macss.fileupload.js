var Macss;
(function (Macss) {
    var FileUpload;
    (function (FileUpload) {
        var MacssFileUpload = /** @class */ (function () {
            function MacssFileUpload(uploadElementSelector, uploadLabelElementSelector, multiple) {
                if (uploadElementSelector === void 0) { uploadElementSelector = ".custom-file-input"; }
                if (uploadLabelElementSelector === void 0) { uploadLabelElementSelector = ".custom-file-label"; }
                if (multiple === void 0) { multiple = false; }
                this.uploadElement = uploadElementSelector;
                this.labelElement = uploadLabelElementSelector;
                this.multipleUpload = multiple;
            }
            MacssFileUpload.prototype.configure = function () {
                var _this = this;
                var upload = this.uploadElement;
                var label = this.labelElement;
                //$(this.uploadElement).on('change', function () {
                //    var fileElement: any = $(upload)[0];
                //    $(upload).next(label).html(fileElement.files[0].name);
                //})
                $(this.uploadElement).on('change', function (e) {
                    //var fileElement: any = this.getFile(); 
                    var str = "";
                    if (_this.multipleUpload) {
                        var num = _this.getFileNum();
                        for (var i = 0; i < num; i++) {
                            str += _this.getFileName(0, i);
                            str += ", ";
                        }
                    }
                    else {
                        str = _this.getFileName();
                    }
                    $(_this.uploadElement).next(_this.labelElement).html(str);
                });
            };
            MacssFileUpload.prototype.getFileNum = function () {
                var elem = this.getFileElement(0);
                return elem.files.length;
            };
            MacssFileUpload.prototype.getFileElement = function (elementIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                return $(this.uploadElement)[elementIndex];
            };
            MacssFileUpload.prototype.getFile = function (elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var elem = this.getFileElement(elementIndex);
                return elem.files[fileIndex];
            };
            MacssFileUpload.prototype.getFileName = function (elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var f = this.getFile(elementIndex, fileIndex);
                if (f == null) {
                    return "";
                }
                return f.name;
            };
            MacssFileUpload.prototype.getFileExt = function (elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var f = this.getFileName(elementIndex, fileIndex);
                var idx = f.lastIndexOf(".");
                if (idx < 0 || idx >= (f.length - 1)) {
                    return "";
                }
                var ext = f.substring(idx);
                return ext;
            };
            MacssFileUpload.prototype.getFileSize = function (elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var f = this.getFile(elementIndex, fileIndex);
                return f.size;
            };
            MacssFileUpload.prototype.acceptExt = function (acceptList, elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var ext = this.getFileExt(elementIndex, fileIndex);
                var idx = acceptList.indexOf(ext);
                return idx >= 0;
            };
            MacssFileUpload.prototype.withinRangeOfFileSize = function (fileSize, elementIndex, fileIndex) {
                if (elementIndex === void 0) { elementIndex = 0; }
                if (fileIndex === void 0) { fileIndex = 0; }
                var fSize = this.getFileSize(elementIndex, fileIndex);
                return fSize < fileSize;
            };
            return MacssFileUpload;
        }());
        FileUpload.MacssFileUpload = MacssFileUpload;
    })(FileUpload = Macss.FileUpload || (Macss.FileUpload = {}));
})(Macss || (Macss = {}));
//# sourceMappingURL=macss.fileupload.js.map