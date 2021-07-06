module Macss.Time {

    export class MacssTime {
        private decId: string; //初期宣言用のID
        private prevTime: string;

        constructor(timeElementId: string) {
            this.decId = timeElementId;
        }

        configure(): void {

            $(this.decId).focus(function () {
                var myId = getMyId(this.id);
                this.prevTime = $(myId).val();               
            });

            $(this.decId).blur(function () {
                var myId = getMyId(this.id);
                var val = $(myId).val();                
                var han;
                var hanNum;
                var time_hh;
                var time_mm;
                var chgFlg = false;
                var editTime;

                if (val.length > 0) {
                    han = val.replace(/[０-９：]/g, function (s) { return String.fromCharCode(s.charCodeAt(0) - 0xFEE0) }); //全角を半角に変換する
                    hanNum = han.replace(':', '');
                    if (isNumber(hanNum) == true && hanNum.length == 4) {
                        time_hh = hanNum.substr(0, 2);
                        time_mm = hanNum.substr(2, 2);
                        if (isTime(time_hh, time_mm) == true) {
                            chgFlg = true;
                            editTime = time_hh + ':' + time_mm;
                        }
                    }
                    if (chgFlg == true) {
                        $(myId).val(editTime);
                    }
                    else {
                        $(myId).val(this.prevTime);
                    }
                }
                else
                { 
                    $(myId).val('');
                }            
            });

            //******************************************
            //* 要素IDを返却する
            //* 引数    val : 入力値
            //* 戻り値  文字列
            //******************************************
            function getMyId(id) {
                return '#' + id;
            }

            //******************************************
            //* 数値チェック
            //* 引数    val : 入力値
            //* 戻り値  数値 : true
            //*	        数値以外 : false
            //******************************************
            function isNumber(val) {
                var pattern = /^\d*$/;
                return pattern.test(val);
            }

            //******************************************
            //* 時間チェック
            //* 引数    time_hh : 数値(時)
            //*         time_mm : 数値(分)
            //* 戻り値  時間 : true
            //*	        時間以外 : false
            //******************************************
            function isTime(time_hh, time_mm) {
                var num_hh = parseInt(time_hh);
                var num_mm = parseInt(time_mm);
                if (num_hh >= 0 && num_hh < 24 && num_mm >= 0 && num_mm < 60) {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
