// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// bootstrap the demo
var demo = new Vue({
    el: '#demo',
    data: {
        searchQuery: '',
        gridColumns: ['name', 'power'],
        gridData: [{
            name: 'Chuck Norris',
            power: 3000
        }, {
            name: 'Bruce Lee',
            power: 9000
        }, {
            name: 'Jackie Chan',
            power: 7000
        }, {
            name: 'Jet Li',
            power: 8000
        }]
    },
    methods: {
        prettyJSON: function (json) {
            if (json) {
                json = JSON.stringify(json, undefined, 4);
                json = json.replace(/&/g, '&').replace(/</g, '<').replace(/>/g, '>');
                return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
                    var cls = 'number';
                    if (/^"/.test(match)) {
                        if (/:$/.test(match)) {
                            cls = 'key';
                        } else {
                            cls = 'string';
                        }
                    } else if (/true|false/.test(match)) {
                        cls = 'boolean';
                    } else if (/null/.test(match)) {
                        cls = 'null';
                    }
                    return '<span class="' + cls + '">' + match + '</span>';
                });
            }
        }
    }
})



Vue.component('cell-grid', {
    template: '#cell-template',
    props: ['value'],
    data: function () {
        return {
            edit: false
        }
    },
    computed: {
        cell: {
            get: function () {
                return this.value
            },
            set: function (newCellValue) {
                this.$emit('input', isNaN(newCellValue) ? newCellValue : parseInt(newCellValue))
            }
        }
    },
    methods: {
        focused: function () {
            console.log('focused')
        },
        editing: function () {
            this.edit = true;
            this.$nextTick(function () {
                this.$refs.cellinput.focus()
            })
        }
    }
})
Vue.component('demo-grid', {
    template: '#grid-template',
    props: {
        data: Array,
        columns: Array,
        filterKey: String
    },
    data: function () {
        var sortOrders = {}
        this.columns.forEach(function (key) {
            sortOrders[key] = 1
        })
        return {
            sortKey: '',
            sortOrders: sortOrders
        }
    },
    computed: {
        filteredData: function () {
            var sortKey = this.sortKey
            var filterKey = this.filterKey && this.filterKey.toLowerCase()
            var order = this.sortOrders[sortKey] || 1
            var data = this.data
            if (filterKey) {
                data = data.filter(function (row) {
                    return Object.keys(row).some(function (key) {
                        return String(row[key]).toLowerCase().indexOf(filterKey) > -1
                    })
                })
            }
            if (sortKey) {
                data = data.slice().sort(function (a, b) {
                    a = a[sortKey]
                    b = b[sortKey]
                    return (a === b ? 0 : a > b ? 1 : -1) * order
                })
            }
            return data
        }
    },
    filters: {
        capitalize: function (str) {
            return str.charAt(0).toUpperCase() + str.slice(1)
        }
    },
    methods: {
        sortBy: function (key) {
            this.sortKey = key
            this.sortOrders[key] = this.sortOrders[key] * -1
        }
    }
})
