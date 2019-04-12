var SchedulerLogMgr = function () {
    var exports = {};
    exports.options = {};
    exports.options.JobId = $.fn.request('keyValue');
    //任务类型
    exports.options.JobStates = {
        1: "label label-success",
        2: "label label-danger",
        3: "label label-warning"
    }
    //初始化数据
    exports.initGridPage = function () {
        //初始化
        $(window).resize(function (e) {
            window.setTimeout(function () {
                $("#layout").css({ "height": $.fn.getLayoutHeight() });
                $('#gridTable').setGridWidth(($('.gridPanel').width()));
                $("#gridTable").setGridHeight($.fn.getGridHeight());
                //$("#itemTree").setTreeHeight($.fn.getLayoutContentHeight());
            }, 100);
            e.stopPropagation();
        });

        //查询条件
        $("#queryCondition .dropdown-menu li").click(function () {
            var text = $(this).find('a').html();
            var value = $(this).find('a').attr('data-value');
            $("#queryCondition .dropdown-text").html(text).attr('data-value', value);
        });

        //查询回车
        $('#txt_Keyword').bind('keypress', function (event) {
            if (event.keyCode == "13") {
                $('.lr-search').trigger("click");
            }
        });
        //注册事件
        $(".titlePanel").on("click", ".lr-replace,.lr-add,.lr-viewlog,.lr-start,.lr-delete,.lr-search", function () {
            var $this = $(this);
            //添加地址
            if ($this.hasClass('lr-replace')) {
                reload();
            }
            else if ($this.hasClass('lr-add')) {
                exports.btnAdd();
            } else if ($this.hasClass('lr-viewlog')) {
                exports.btnViewLogDetail();
            }
            else if ($this.hasClass('lr-start')) {
                exports.btnEnabled();
            }
            else if ($this.hasClass('lr-delete')) {
                exports.btnDelete();
            }
            else if ($this.hasClass('lr-search')) {
                exports.SearchEvent(0);
            }
        });

    };

    //加载树
    exports.loadTree = function () {
        var item = {
            height: $.fn.getLayoutContentHeight(),
            url: "/content/supermgr/json/ModuleTree.json",
            onnodeclick: function (item) {
                exports.options.ParentId = item.id;
                $('.lr-search').trigger("click");
            }
        };
        //初始化
        $("#itemTree").treeview(item);
    };

    //加载Grid
    exports.loadGrid = function () {
        var selectedRowIndex = 0;
        exports.options.$gridTable = $("#gridTable");
        exports.options.$gridTable.jqGrid({
            datatype: "json",
            mtype: "POST",
            url: '/UpgradePlan/Index',
            height: $.fn.getGridHeight(),
            autowidth: true,
            multiselect: false,
            colModel: [
                { label: "主键", name: "plan_id", index: "plan_id", hidden: true },
                { label: "部署点", name: "server_id", index: "server_id", hidden: true },
                { label: "序号", name: "displayid", index: "displayid", width: 50 },
                { label: "名称", name: "name", index: "name", width: 120, align: "left" },
                { label: "部署点名称", name: "server_name", index: "server_name", width: 120, align: "left" },
                { label: "升级版本", name: "epms_info_id", index: "epms_info_id", width: 120, align: "left" },
                { label: "数据库脚本", name: "dbsql_version_id", index: "dbsql_version_id", width: 120, align: "left" },
                { label: "母机", name: "source_host_id", index: "source_host_id", width: 100, align: "center" },
                { label: "状态", name: "status", index: "status", width: 70, align: "center" },
                { label: "进度", name: "upgrade_progress", index: "upgrade_progress", width: 120, align: "center" },
                {
                    label: "升级时间", name: "upgrade_date", index: "upgrade_date", width: 120, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        return formatDate(cellvalue, 'MM-dd hh:mm:ss');
                    }
                },
                {
                    label: "完成时间", name: "upgrade_finish_date", index: "upgrade_finish_date", width: 120, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        return formatDate(cellvalue, 'MM-dd hh:mm:ss');
                    }
                },
                {
                    label: "升级网站", name: "is_upgrade_epms", index: "is_upgrade_epms", width: 80, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        var index = rowObject['is_upgrade_epms'];
                        if (index == 1) {
                            return '<i value="' + cellvalue + '" class="fa fa-toggle-on"></i>';
                        } else {
                            return '<i value=' + cellvalue + '" class="fa fa-toggle-off"></i>';
                        }
                    }
                },
                {
                    label: "同步结构", name: "is_upgrade_dbstruct", index: "is_upgrade_dbstruct", width: 80, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        var index = rowObject['is_upgrade_dbstruct'];
                        if (index == 1) {
                            return '<i value="' + cellvalue + '" class="fa fa-toggle-on"></i>';
                        } else {
                            return '<i value=' + cellvalue + '" class="fa fa-toggle-off"></i>';
                        }
                    }
                },
                {
                    label: "执行脚本", name: "is_upgrade_dbdata", index: "is_upgrade_dbdata", width: 80, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        var index = rowObject['is_upgrade_dbdata'];
                        if (index == 1) {
                            return '<i value="' + cellvalue + '" class="fa fa-toggle-on"></i>';
                        } else {
                            return '<i value=' + cellvalue + '" class="fa fa-toggle-off"></i>';
                        }
                    }
                },
                {
                    label: "基础数据", name: "is_send_data", index: "is_send_data", width: 80, align: "center",
                    formatter: function (cellvalue, options, rowObject) {
                        var index = rowObject['is_send_data'];
                        if (index == 1) {
                            return '<i value="' + cellvalue + '" class="fa fa-toggle-on"></i>';
                        } else {
                            return '<i value=' + cellvalue + '" class="fa fa-toggle-off"></i>';
                        }
                    }
                },
                { label: "创建人", name: "create_user", index: "create_user", width: 100, align: "center" }
            ],
            pager: "#gridPager",
            sortname: 'displayid',
            sortorder: "desc",
            rowList: [20, 50],
            rowNum: "20",
            onSelectRow: function () {
                selectedRowIndex = $("#" + this.id).getGridParam('selrow');
            },
            loadError: function (xhr, status, error) {
                switch (error) {
                    case "Unauthorized":
                        top.location.href = "/account/login";
                        break;
                    default:
                        $.fn.modalMsg(error, "error");
                        break;
                }
            }
        });
        exports.SearchEvent();
    }

    //设置列的值
    exports.setCellValue = function (id, data) {
        $jgrid = $("#gridTable");
        var index = $jgrid.jqGridIndex("plan_id", id);
        //状态
        if (data.message == "升级成功") {
            //status upgrade_finish_date
            $jgrid.setCell(index, "upgrade_finish_date", data.upgrade_finish_date);
            $jgrid.setCell(index, "status", "已完成");
        } else {
            $jgrid.setCell(index, "status", "已下达");
        }
        //进度
        $jgrid.setCell(index, "upgrade_progress", data.message);
    }
    //更新行的值
    exports.setRowData = function (id, data) {
        $("#gridTable").jqGridSetRowData(id, value);
    }

    //查询表格函数
    exports.SearchEvent = function () {
        var queryJson = $("#form1").GetWebControls();
        var jobState = $("#queryCondition .dropdown-text").attr('data-value');
        queryJson["Status"] = jobState;
        $("#gridTable").jqGrid('setGridParam', {
            url: '/UpgradePlan/Index',
            postData: queryJson,
            page: 1
        }).trigger('reloadGrid');
    }

    //删除
    exports.btnDelete = function () {
        var $jgrid = $("#gridTable");
        var planId = $jgrid.jqGridRowValue("plan_id");
        var serverId = $jgrid.jqGridRowValue("server_id");
        if (planId) {
            $.fn.deleteForm({
                url: "/UpgradePlan/Remove",
                param: { planId: planId, serverId: serverId },
                success: function (data) {
                    if (data.success) {
                        $jgrid.trigger("reloadGrid");
                        $jgrid.resetSelection();
                    } else {
                        $.fn.modalMsg(data.message, "error");
                    }
                }
            });
        } else {
            $.fn.modalMsg('请选择需要删除的数据项！', "warning");
        }
    }

    //重新升级
    exports.btnEnabled = function () {
        var $jgrid = $("#gridTable");
        var planId = $jgrid.jqGridRowValue("plan_id");
        if (planId) {
            if (checkedRow(planId)) {
                $.fn.submitAjax({
                    url: "/UpgradePlan/ReUpgrade",
                    param: { id: planId },
                    success: function (data) {
                        if (data.success) {
                            $jgrid.trigger("reloadGrid");
                            $jgrid.resetSelection();
                        } else {
                            $.fn.modalMsg(data.message, "error");
                        }
                    }
                });
            }
        } else {
            $.fn.modalMsg('请选择需要重新升级的数据项！', "warning");
        }
    }

    //查看任务日志详情
    exports.btnViewLogDetail = function () {
        var keyValue = $("#gridTable").jqGridRowValue("server_id");
        if (checkedRow(keyValue)) {
            $.fn.modalOpen({
                id: "LogDetailForm",
                title: '查看任务日志详情',
                url: '/UpgradePlan/LogDetail/' + keyValue,
                width: "800px",
                height: "680px",
                btn: ['查询', '关闭'],
                callBack: function (iframeId) {
                    top.frames[iframeId].AcceptClick();
                }
            });
        }
        else {
            $.fn.modalMsg('请选择需要查看的数据项！', "warning");
        }
    }

    //添加
    exports.btnAdd = function () {
        $.fn.modalOpen({
            id: "UpgradePlanForm",
            title: '添加升级计划',
            url: '/UpgradePlan/CreatePlan',
            width: window.innerWidth + 'px',
            height: window.innerHeight + 'px',
            callBack: function (iframeId) {
                top.frames[iframeId].AcceptClick();
            }
        });
    }

    return exports;
};