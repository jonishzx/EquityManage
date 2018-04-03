(function (b) {
    var a = {};
    a.config = {
        editable: true,
        showtip: true,
        lineHeight: 15,
        basePath: "",
        rect: {
            attr: {
                x: 10,
                y: 10,
                width: 100,
                height: 50,
                r: 5,
                fill: "90-#fff-#C0C0C0",
                stroke: "#000",
                "stroke-width": 1
            },
            showType: "image\x26text",
            type: "state",
            name: {
                text: "state",
                "font-style": "italic"
            },
            text: {
                text: "状态",
                "font-size": 13
            },
            margin: 5,
            props: [],
            img: {}

        },
        addRectHandler: function () { },
        removeRectHandler: function () { },
        path: {
            attr: {
                path: {
                    path: "M10 10L100 100",
                    stroke: "#808080",
                    fill: "none",
                    "stroke-width": 5,
                    cursor: "pointer"
                },
                arrow: {
                    path: "M10 10L10 10",
                    stroke: "#808080",
                    fill: "#808080",
                    "stroke-width": 4,
                    radius: 5
                },
                fromDot: {
                    width: 6,
                    height: 6,
                    stroke: "#fff",
                    fill: "#000",
                    cursor: "move",
                    "stroke-width": 2
                },
                toDot: {
                    width: 6,
                    height: 6,
                    stroke: "#fff",
                    fill: "#000",
                    cursor: "move",
                    "stroke-width": 2
                },
                bigDot: {
                    width: 7,
                    height: 7,
                    stroke: "#fff",
                    fill: "#000",
                    cursor: "move",
                    "stroke-width": 2
                },
                smallDot: {
                    width: 6,
                    height: 6,
                    stroke: "#fff",
                    fill: "#000",
                    cursor: "move",
                    "stroke-width": 3
                }
            },
            text: {
                text: " ",
                cursor: "move",
                background: "#000",
                "font-size": "12px"
            },
            textPos: {
                x: 0,
                y: -10
            },
            props: {
                text: {
                    name: "DisplayName",
                    label: "显示",
                    value: "",
                    editor: function () {
                        return new a.editors["textEditor"]()
                    }
                }
            },
            mouseover: function () { }

        },
        tools: {
            attr: {
                left: 10,
                top: 10
            },
            pointer: {},
            path: {},
            states: {},
            save: {
                onclick: function (c) {
                    alert(c)
                }
            },
            xml: {
                onclick: function (c) {
                    alert(c)
                }
            }
        },
        props: {
            attr: {
                top: 10,
                right: 30
            },
            props: {}

        },
        restore: "",
        activeRects: {
            rects: [],
            rectAttr: {
                stroke: "#ff0000",
                "stroke-width": 2
            }
        },
        historyRects: {
            rects: [],
            rectAttr: {
                stroke: "#C0C0C0",
                "stroke-width": 2
            },
            pathAttr: {
                path: {
                    stroke: "#C0C0C0"
                },
                arrow: {
                    stroke: "#C0C0C0",
                    fill: "#C0C0C0"
                }
            }
        }
    };
    a.util = {
        isLine: function (f, e, d) {
            var c,
			g;
            if ((f.x - d.x) == 0) {
                c = 1
            } else {
                c = (f.y - d.y) / (f.x - d.x)
            }
            g = (e.x - d.x) * c + d.y;
            if ((e.y - g) < 10 && (e.y - g) > -10) {
                e.y = g;
                return true
            }
            return false
        },
        center: function (c, d) {
            return {
                x: (c.x - d.x) / 2 + d.x,
                y: (c.y - d.y) / 2 + d.y
            }
        },
        nextId: (function () {
            var c = 0;
            return function () {
                return ++c
            }
        })(),
        connPoint: function (f, g) {
            var c = g,
			h = {
			    x: f.x + f.width / 2,
			    y: f.y + f.height / 2
			};
            var e = (h.y - c.y) / (h.x - c.x);
            e = isNaN(e) ? 0 : e;
            var d = f.height / f.width;
            var l = c.y < h.y ? -1 : 1,
			j = c.x < h.x ? -1 : 1,
			k,
			i;
            if (Math.abs(e) > d && l == -1) {
                k = h.y - f.height / 2;
                i = h.x + l * f.height / 2 / e
            } else {
                if (Math.abs(e) > d && l == 1) {
                    k = h.y + f.height / 2;
                    i = h.x + l * f.height / 2 / e
                } else {
                    if (Math.abs(e) < d && j == -1) {
                        k = h.y + j * f.width / 2 * e;
                        i = h.x - f.width / 2
                    } else {
                        if (Math.abs(e) < d && j == 1) {
                            k = h.y + f.width / 2 * e;
                            i = h.x + f.width / 2
                        }
                    }
                }
            }
            return {
                x: i,
                y: k
            }
        },
        arrow: function (e, d, g) {
            var k = Math.atan2(e.y - d.y, d.x - e.x) * (180 / Math.PI);
            var l = d.x - g * Math.cos(k * (Math.PI / 180));
            var j = d.y + g * Math.sin(k * (Math.PI / 180));
            var h = l + g * Math.cos((k + 120) * (Math.PI / 180));
            var f = j - g * Math.sin((k + 120) * (Math.PI / 180));
            var c = l + g * Math.cos((k + 240) * (Math.PI / 180));
            var i = j - g * Math.sin((k + 240) * (Math.PI / 180));
            return [d, {
                x: h,
                y: f
            }, {
                x: c,
                y: i
            }
			]
        }
    };
    a.rect = function (A, y) {
        var w = this,
		B = "rect" + a.util["nextId"](),
		u = b.extend(true, {}, a.config["rect"], A),
		n = y,
		m,
		l,
		k,
		i,
		g,
		e;
        m = n.rect(u.attr["x"], u.attr["y"], u.attr["width"], u.attr["height"], u.attr["r"])["hide"]()["attr"](u.attr);
        l = n.image(a.config["basePath"] + u.img["src"], u.attr["x"] + u.img["width"] / 2, u.attr["y"] + (u.attr["height"] - u.img["height"]) / 2, u.img["width"], u.img["height"])["hide"]();
        k = n.text(u.attr["x"] + u.img["width"] + (u.attr["width"] - u.img["width"]) / 2, u.attr["y"] + a.config["lineHeight"] / 2, u.name["text"])["hide"]()["attr"](u.name);
        i = n.text(u.attr["x"] + u.img["width"] + (u.attr["width"] - u.img["width"]) / 2, u.attr["y"] + (u.attr["height"] - a.config["lineHeight"]) / 2 + a.config["lineHeight"], u.text["text"])["hide"]()["attr"](u.text);
        m.drag(function (H, G) {
            c(H, G)
        }, function () {
            j()
        }, function () {
            h()
        });
        l.drag(function (H, G) {
            c(H, G)
        }, function () {
            j()
        }, function () {
            h()
        });
        k.drag(function (H, G) {
            c(H, G)
        }, function () {
            j()
        }, function () {
            h()
        });
        i.drag(function (H, G) {
            c(H, G)
        }, function () {
            j()
        }, function () {
            h()
        });
        var c = function (J, H) {
            if (!a.config["editable"]) {
                return
            }
            p();
            var G = (g + J);
            var I = (e + H);
            d.x = G - u.margin;
            d.y = I - u.margin;
            o()
        };
        var j = function () {
            g = m.attr("x");
            e = m.attr("y");
            m.attr({
                opacity: 0.5
            });
            l.attr({
                opacity: 0.5
            });
            i.attr({
                opacity: 0.5
            })
        };
        var h = function () {
            m.attr({
                opacity: 1
            });
            l.attr({
                opacity: 1
            });
            i.attr({
                opacity: 1
            })
        };
        var f,
		C = {},
		F = 5,
		d = {
		    x: u.attr["x"] - u.margin,
		    y: u.attr["y"] - u.margin,
		    width: u.attr["width"] + u.margin * 2,
		    height: u.attr["height"] + u.margin * 2
		};
        f = n.path("M0 0L1 1")["hide"]();
        C.t = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "s-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "t")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "t")
        }, function () { });
        C.lt = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "nw-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "lt")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "lt")
        }, function () { });
        C.l = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "w-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "l")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "l")
        }, function () { });
        C.lb = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "sw-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "lb")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "lb")
        }, function () { });
        C.b = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "s-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "b")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "b")
        }, function () { });
        C.rb = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "se-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "rb")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "rb")
        }, function () { });
        C.r = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "w-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "r")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "r")
        }, function () { });
        C.rt = n.rect(0, 0, F, F)["attr"]({
            fill: "#000",
            stroke: "#fff",
            cursor: "ne-resize"
        })["hide"]()["drag"](function (H, G) {
            E(H, G, "rt")
        }, function () {
            r(this["attr"]("x") + F / 2, this["attr"]("y") + F / 2, "rt")
        }, function () { });
        var E = function (K, I, J) {
            if (!a.config["editable"]) {
                return
            }
            var H = _bx + K,
			G = _by + I;
            switch (J) {
                case "t":
                    d.height += d.y - G;
                    d.y = G;
                    break;
                case "lt":
                    d.width += d.x - H;
                    d.height += d.y - G;
                    d.x = H;
                    d.y = G;
                    break;
                case "l":
                    d.width += d.x - H;
                    d.x = H;
                    break;
                case "lb":
                    d.height = G - d.y;
                    d.width += d.x - H;
                    d.x = H;
                    break;
                case "b":
                    d.height = G - d.y;
                    break;
                case "rb":
                    d.height = G - d.y;
                    d.width = H - d.x;
                    break;
                case "r":
                    d.width = H - d.x;
                    break;
                case "rt":
                    d.width = H - d.x;
                    d.height += d.y - G;
                    d.y = G;
                    break
            }
            o()
        };
        var r = function (H, G, I) {
            _bx = H;
            _by = G
        };
        b([m.node, i.node, k.node, l.node])["bind"]("click", function () {
            if (!a.config["editable"]) {
                return
            }
            q();
            var G = b(n)["data"]("mod");
            switch (G) {
                case "pointer":
                    break;
                case "path":
                    var H = b(n)["data"]("currNode");
                    if (H && H.getId() != B && H.getId()["substring"](0, 4) == "rect") {
                        b(n)["trigger"]("addpath", [H, w])
                    }
                    break
            }
            b(n)["trigger"]("click", w);
            b(n)["data"]("currNode", w);
            return false
        });
        var z = function (G, H) {
            if (a.config["showtip"]) {
                if (H.getId() == B) {
                    b(n)["trigger"]("showprops", [u.props, H])
                } else {
                    p()
                }
            }
            if (!a.config["editable"]) {
                return
            }
            if (H.getId() == B) {
                b(n)["trigger"]("showprops", [u.props, H])
            } else {
                p()
            }
        };
        b(n)["bind"]("click", z);
        if (u.dbclick) {
            i.dblclick(x);
            m.dblclick(x);
            l.dblclick(x)
        }
        if (u.mouseover) {
            i.mouseover(v);
            m.mouseover(v);
            l.mouseover(v)
        }
        if (u.mouseout) {
            i.mouseout(t);
            m.mouseout(t);
            l.mouseout(t)
        }
        function x() {
            u.dbclick(u, this)
        }
        function v() {
            u.mouseover(u, this)
        }
        function t() {
            u.mouseout(u, this)
        }
        var D = function (G, I, H) {
            if (H.getId() == B) {
                i.attr({
                    text: I
                })
            }
        };
        b(n)["bind"]("textchange", D);
        function s() {
            return "M" + d.x + " " + d.y + "L" + d.x + " " + (d.y + d.height) + "L" + (d.x + d.width) + " " + (d.y + d.height) + "L" + (d.x + d.width) + " " + d.y + "L" + d.x + " " + d.y
        }
        function q() {
            f.show();
            for (var G in C) {
                C[G]["show"]()
            }
        }
        function p() {
            f.hide();
            for (var G in C) {
                C[G]["hide"]()
            }
        }
        function o() {
            var J = d.x + u.margin,
			H = d.y + u.margin,
			I = d.width - u.margin * 2,
			G = d.height - u.margin * 2;
            m.attr({
                x: J,
                y: H,
                width: I,
                height: G
            });
            switch (u.showType) {
                case "image":
                    l.attr({
                        x: J + (I - u.img["width"]) / 2,
                        y: H + (G - u.img["height"]) / 2
                    })["show"]();
                    break;
                case "text":
                    m.show();
                    i.attr({
                        x: J + I / 2,
                        y: H + G / 2
                    })["show"]();
                    break;
                case "image\x26text":
                    m.show();
                    i.attr({
                        x: J + I / 2,
                        y: H + (G - a.config["lineHeight"]) / 2 + a.config["lineHeight"] + 10
                    })["show"]();
                    l.attr({
                        x: J + u.img["width"] / 2,
                        y: H + (G - u.img["height"]) / 15
                    })["show"]();
                    break
            }
            C.t["attr"]({
                x: d.x + d.width / 2 - F / 2,
                y: d.y - F / 2
            });
            C.lt["attr"]({
                x: d.x - F / 2,
                y: d.y - F / 2
            });
            C.l["attr"]({
                x: d.x - F / 2,
                y: d.y - F / 2 + d.height / 2
            });
            C.lb["attr"]({
                x: d.x - F / 2,
                y: d.y - F / 2 + d.height
            });
            C.b["attr"]({
                x: d.x - F / 2 + d.width / 2,
                y: d.y - F / 2 + d.height
            });
            C.rb["attr"]({
                x: d.x - F / 2 + d.width,
                y: d.y - F / 2 + d.height
            });
            C.r["attr"]({
                x: d.x - F / 2 + d.width,
                y: d.y - F / 2 + d.height / 2
            });
            C.rt["attr"]({
                x: d.x - F / 2 + d.width,
                y: d.y - F / 2
            });
            f.attr({
                path: s()
            });
            b(n)["trigger"]("rectresize", w)
        }
        this["toJson"] = function () {
            var H = "{type:\x27" + u.type + "\x27,text:{text:\x27" + i.attr("text") + "\x27}, attr:{ x:" + Math.round(m.attr("x")) + ", y:" + Math.round(m.attr("y")) + ", width:" + Math.round(m.attr("width")) + ", height:" + Math.round(m.attr("height")) + "}, props:{";
            for (var G in u.props) {
                H += G + ":{value:\x27" + u.props[G]["value"] + "\x27},"
            }
            if (H.substring(H.length - 1, H.length) == ",") {
                H = H.substring(0, H.length - 1)
            }
            H += "}}";
            return H
        };
        this["restore"] = function (G) {
            var H = G;
            u = b.extend(true, u, G);
            i.attr({
                text: H.text["text"]
            });
            o()
        };
        this["getBBox"] = function () {
            return d
        };
        this["getId"] = function () {
            return B
        };
        this["remove"] = function () {
            m.remove();
            i.remove();
            k.remove();
            l.remove();
            f.remove();
            for (var G in C) {
                C[G]["remove"]()
            }
        };
        this["uiobject"] = function () {
            return u
        };
        this["text"] = function () {
            return i.attr("text")
        };
        this["setText"] = function (G) {
            i.attr({
                text: G
            });
            o()
        };
        this["attr"] = function (G) {
            if (G) {
                if (l && u.showType == "image") {
                    if (b.browser["msie"] && parseInt(b.browser["version"], 10) < 9) {
                        delete G.fill
                    }
                    l.attr(G)
                }
                m.attr(G)
            }
        };
        o()
    };
    a.path = function (t, r, o, l) {
        var j = this,
		r = r,
		d = b.extend(true, {}, a.config["path"]),
		c,
		B,
		A,
		w = d.textPos,
		k,
		i,
		z = o,
		y = l,
		u = "path" + a.util["nextId"](),
		h;
        function x(H, E, N, D) {
            var C = this,
			L = H,
			O,
			M = N,
			K = D,
			J,
			I,
			G = E;
            switch (L) {
                case "from":
                    O = r.rect(E.x - d.attr["fromDot"]["width"] / 2, E.y - d.attr["fromDot"]["height"] / 2, d.attr["fromDot"]["width"], d.attr["fromDot"]["height"])["attr"](d.attr["fromDot"]);
                    break;
                case "big":
                    O = r.rect(E.x - d.attr["bigDot"]["width"] / 2, E.y - d.attr["bigDot"]["height"] / 2, d.attr["bigDot"]["width"], d.attr["bigDot"]["height"])["attr"](d.attr["bigDot"]);
                    break;
                case "small":
                    O = r.rect(E.x - d.attr["smallDot"]["width"] / 2, E.y - d.attr["smallDot"]["height"] / 2, d.attr["smallDot"]["width"], d.attr["smallDot"]["height"])["attr"](d.attr["smallDot"]);
                    break;
                case "to":
                    O = r.rect(E.x - d.attr["toDot"]["width"] / 2, E.y - d.attr["toDot"]["height"] / 2, d.attr["toDot"]["width"], d.attr["toDot"]["height"])["attr"](d.attr["toDot"]);
                    break
            }
            if (O && (L == "big" || L == "small")) {
                O.drag(function (R, S) {
                    F(R, S)
                }, function () {
                    Q()
                }, function () {
                    P()
                });
                var F = function (T, S) {
                    var U = (J + T),
					R = (I + S);
                    C.moveTo(U, R)
                };
                var Q = function () {
                    if (L == "big") {
                        J = O.attr("x") + d.attr["bigDot"]["width"] / 2;
                        I = O.attr("y") + d.attr["bigDot"]["height"] / 2
                    }
                    if (L == "small") {
                        J = O.attr("x") + d.attr["smallDot"]["width"] / 2;
                        I = O.attr("y") + d.attr["smallDot"]["height"] / 2
                    }
                };
                var P = function () { }

            }
            this["type"] = function (R) {
                if (R) {
                    L = R
                } else {
                    return L
                }
            };
            this["node"] = function (R) {
                if (R) {
                    O = R
                } else {
                    return O
                }
            };
            this["left"] = function (R) {
                if (R) {
                    M = R
                } else {
                    return M
                }
            };
            this["right"] = function (R) {
                if (R) {
                    K = R
                } else {
                    return K
                }
            };
            this["remove"] = function () {
                M = null;
                K = null;
                O.remove()
            };
            this["pos"] = function (R) {
                if (R) {
                    G = R;
                    O.attr({
                        x: G.x - O.attr("width") / 2,
                        y: G.y - O.attr("height") / 2
                    });
                    return this
                } else {
                    return G
                }
            };
            this["moveTo"] = function (U, R) {
                this["pos"]({
                    x: U,
                    y: R
                });
                switch (L) {
                    case "from":
                        if (K && K.right() && K.right()["type"]() == "to") {
                            K.right()["pos"](a.util["connPoint"](y.getBBox(), G))
                        }
                        if (K && K.right()) {
                            K.pos(a.util["center"](G, K.right()["pos"]()))
                        }
                        break;
                    case "big":
                        if (K && K.right() && K.right()["type"]() == "to") {
                            K.right()["pos"](a.util["connPoint"](y.getBBox(), G))
                        }
                        if (M && M.left() && M.left()["type"]() == "from") {
                            M.left()["pos"](a.util["connPoint"](z.getBBox(), G))
                        }
                        if (K && K.right()) {
                            K.pos(a.util["center"](G, K.right()["pos"]()))
                        }
                        if (M && M.left()) {
                            M.pos(a.util["center"](G, M.left()["pos"]()))
                        }
                        var S = {
                            x: G.x,
                            y: G.y
                        };
                        if (a.util["isLine"](M.left()["pos"](), S, K.right()["pos"]())) {
                            L = "small";
                            O.attr(d.attr["smallDot"]);
                            this["pos"](S);
                            var V = M;
                            M.left()["right"](M.right());
                            M = M.left();
                            V.remove();
                            var T = K;
                            K.right()["left"](K.left());
                            K = K.right();
                            T.remove()
                        }
                        break;
                    case "small":
                        if (M && K && !a.util["isLine"](M.pos(), {
                            x: G.x,
                            y: G.y
                        }, K.pos())) {
                            L = "big";
                            O.attr(d.attr["bigDot"]);
                            var V = new x("small", a.util["center"](M.pos(), G), M, M.right());
                            M.right(V);
                            M = V;
                            var T = new x("small", a.util["center"](K.pos(), G), K.left(), K);
                            K.left(T);
                            K = T
                        }
                        break;
                    case "to":
                        if (M && M.left() && M.left()["type"]() == "from") {
                            M.left()["pos"](a.util["connPoint"](z.getBBox(), G))
                        }
                        if (M && M.left()) {
                            M.pos(a.util["center"](G, M.left()["pos"]()))
                        }
                        break
                }
                s()
            }
        }
        function n() {
            var E,
			C,
			D = z.getBBox(),
			H = y.getBBox(),
			G,
			F;
            G = a.util["connPoint"](D, {
                x: H.x + H.width / 2,
                y: H.y + H.height / 2
            });
            F = a.util["connPoint"](H, G);
            E = new x("from", G, null, new x("small", {
                x: (G.x + F.x) / 2,
                y: (G.y + F.y) / 2
            }));
            E.right()["left"](E);
            C = new x("to", F, E.right(), null);
            E.right()["right"](C);
            this["toPathString"] = function () {
                if (!E) {
                    return ""
                }
                var J = E,
				L = "M" + J.pos()["x"] + " " + J.pos()["y"],
				I = "";
                while (J.right()) {
                    J = J.right();
                    L += "L" + J.pos()["x"] + " " + J.pos()["y"]
                }
                var K = a.util["arrow"](J.left()["pos"](), J.pos(), d.attr["arrow"]["radius"]);
                I = "M" + K[0]["x"] + " " + K[0]["y"] + "L" + K[1]["x"] + " " + K[1]["y"] + "L" + K[2]["x"] + " " + K[2]["y"] + "z";
                return [L, I]
            };
            this["toJson"] = function () {
                var J = "[",
				I = E;
                while (I) {
                    if (I.type() == "big") {
                        J += "{x:" + Math.round(I.pos()["x"]) + ",y:" + Math.round(I.pos()["y"]) + "},"
                    }
                    I = I.right()
                }
                if (J.substring(J.length - 1, J.length) == ",") {
                    J = J.substring(0, J.length - 1)
                }
                J += "]";
                return J
            };
            this["restore"] = function (I) {
                var L = I,
				J = E.right();
                for (var K = 0; K < L.length; K++) {
                    J.moveTo(L[K]["x"], L[K]["y"]);
                    J.moveTo(L[K]["x"], L[K]["y"]);
                    J = J.right()
                }
                this["hide"]()
            };
            this["fromDot"] = function () {
                return E
            };
            this["toDot"] = function () {
                return C
            };
            this["midDot"] = function () {
                var I = E.right(),
				J = E.right()["right"]();
                while (J.right() && J.right()["right"]()) {
                    J = J.right()["right"]();
                    I = I.right()
                }
                return I
            };
            this["show"] = function () {
                var I = E;
                while (I) {
                    I.node()["show"]();
                    I = I.right()
                }
            };
            this["hide"] = function () {
                var I = E;
                while (I) {
                    I.node()["hide"]();
                    I = I.right()
                }
            };
            this["remove"] = function () {
                var I = E;
                while (I) {
                    if (I.right()) {
                        I = I.right();
                        I.left()["remove"]()
                    } else {
                        I.remove();
                        I = null
                    }
                }
            }
        }
        d = b.extend(true, d, t);
        c = r.path(d.attr["path"]["path"])["attr"](d.attr["path"]);
        B = r.path(d.attr["arrow"]["path"])["attr"](d.attr["arrow"]);
        h = new n();
        h.hide();
        var g = d.text["text"]["replace"]("{from}", z.text())["replace"]("{to}", y.text());
        A = r.text(0, 0, d.text["text"])["attr"](d.text)["attr"]({
            text: d.text.text
        });
        if (d.mouseover) {
            c.mouseover(q);
            B.mouseover(q)
        }
        if (d.mouseout) {
            c.mouseout(m);
            B.mouseout(m)
        }
        function q() {
            d.mouseover(d, this)
        }
        function m() {
            d.mouseout(d, this)
        }
        A.drag(function (D, C) {
            if (!a.config["editable"]) {
                return
            }
            A.attr({
                x: k + D,
                y: i + C
            })
        }, function () {
            k = A.attr("x");
            i = A.attr("y")
        }, function () {
            var C = h.midDot()["pos"]();
            w = {
                x: A.attr("x") - C.x,
                y: A.attr("y") - C.y
            }
        });
        s();
        b([c.node, B.node])["bind"]("click", function () {
            if (!a.config["editable"]) {
                return
            }
            b(r)["trigger"]("click", j);
            b(r)["data"]("currNode", j);
            return false
        });
        var f = function (E, C) {
            if (!a.config["editable"]) {
                return
            }
            if (C && C.getId() == u) {
                h.show();
                b(r)["trigger"]("showprops", [d.props, j])
            } else {
                h.hide()
            }
            var D = b(r)["data"]("mod");
            switch (D) {
                case "pointer":
                    break;
                case "path":
                    break
            }
        };
        b(r)["bind"]("click", f);
        var e = function (C, D) {
            if (!a.config["editable"]) {
                return
            }
            if (D && (D.getId() == z.getId() || D.getId() == y.getId())) {
                b(r)["trigger"]("removepath", j)
            }
        };
        b(r)["bind"]("removerect", e);
        var p = function (C, D) {
            if (!a.config["editable"]) {
                return
            }
            if (z && z.getId() == D.getId()) {
                var E;
                if (h.fromDot()["right"]()["right"]()["type"]() == "to") {
                    E = {
                        x: y.getBBox()["x"] + y.getBBox()["width"] / 2,
                        y: y.getBBox()["y"] + y.getBBox()["height"] / 2
                    }
                } else {
                    E = h.fromDot()["right"]()["right"]()["pos"]()
                }
                var F = a.util["connPoint"](z.getBBox(), E);
                h.fromDot()["moveTo"](F.x, F.y);
                s()
            }
            if (y && y.getId() == D.getId()) {
                var E;
                if (h.toDot()["left"]()["left"]()["type"]() == "from") {
                    E = {
                        x: z.getBBox()["x"] + z.getBBox()["width"] / 2,
                        y: z.getBBox()["y"] + z.getBBox()["height"] / 2
                    }
                } else {
                    E = h.toDot()["left"]()["left"]()["pos"]()
                }
                var F = a.util["connPoint"](y.getBBox(), E);
                h.toDot()["moveTo"](F.x, F.y);
                s()
            }
        };
        b(r)["bind"]("rectresize", p);
        var v = function (E, D, C) {
            if (C.getId() == u) {
                if (!D) {
                    D = " "
                }
                A.attr({
                    text: D
                })
            }
        };
        b(r)["bind"]("textchange", v);
        this["from"] = function () {
            return z
        };
        this["to"] = function () {
            return y
        };
        this["toJson"] = function () {
            var D = "{from:\x27" + z.getId() + "\x27,to:\x27" + y.getId() + "\x27, dots:" + h.toJson() + ",text:{text:\x27" + A.attr("text") + "\x27},textPos:{x:" + Math.round(w.x) + ",y:" + Math.round(w.y) + "}, props:{";
            for (var C in d.props) {
                D += C + ":{value:\x27" + d.props[C]["value"] + "\x27},"
            }
            if (D.substring(D.length - 1, D.length) == ",") {
                D = D.substring(0, D.length - 1)
            }
            D += "}}";
            return D
        };
        this["restore"] = function (C) {
            var D = C;
            d = b.extend(true, d, C);
            h.restore(D.dots)
        };
        this["uiobject"] = function () {
            return d
        };
        this["remove"] = function () {
            h.remove();
            c.remove();
            B.remove();
            A.remove();
            try {
                b(r)["unbind"]("click", f)
            } catch (C) { }

            try {
                b(r)["unbind"]("removerect", e)
            } catch (C) { }

            try {
                b(r)["unbind"]("rectresize", p)
            } catch (C) { }

            try {
                b(r)["unbind"]("textchange", v)
            } catch (C) { }

        };
        function s() {
            var D = h.toPathString(),
			C = h.midDot()["pos"]();
            c.attr({
                path: D[0]
            });
            B.attr({
                path: D[1]
            });
            A.attr({
                x: C.x + w.x,
                y: C.y + w.y
            })
        }
        this["getId"] = function () {
            return u
        };
        this["text"] = function () {
            return A.attr("text")
        };
        this["setText"] = function (C) {
            d.text["text"] = C;
            A.attr({
                text: C
            })
        };
        this["attr"] = function (C) {
            if (C && C.path) {
                c.attr(C.path)
            }
            if (C && C.arrow) {
                B.attr(C.arrow)
            }
        }
    };
    a.props = function (f, g) {
        var c = this,
		j = b("#myflow_props")["hide"]()["draggable"]({
		    handle: "#myflow_props_handle"
		})["resizable"]()["css"](a.config["props"]["attr"])["bind"]("click", function () {
		    return false
		}),
		e = j.find("table"),
		h = g,
		d;
        var i = function (n, l, m) {
            if (d && d.getId() == m.getId()) {
                return
            }
            d = m;
            b(e)["find"](".editor")["each"](function () {
                var o = b(this)["data"]("editor");
                if (o) {
                    o.destroy()
                }
            });
            e.empty();
            j.show();
            for (var k in l) {
                if (l[k]["label"]) {
                    e.append("\x3Ctr\x3E\x3Cth\x3E" + l[k]["label"] + "\x3C/th\x3E\x3Ctd\x3E\x3Cdiv id=\x22p" + k + "\x22 class=\x22editor\x22\x3E\x3C/div\x3E\x3C/td\x3E\x3C/tr\x3E");
                    if (l[k]["editor"]) {
                        l[k]["editor"]()["init"](l, k, "p" + k, m, h)
                    }
                }
            }
        };
        b(h)["bind"]("showprops", i)
    };
    a.editors = {
        textEditor: function () {
            var c,
			d,
			g,
			f,
			e;
            this["init"] = function (k, l, i, h, j) {
                c = k;
                d = l;
                g = i;
                f = h;
                e = j;
                b("\x3Cinput  style=\x22width:100%;\x22/\x3E")["val"](f.text())["change"](function () {
                    k[d]["value"] = b(this)["val"]();
                    b(e)["trigger"]("textchange", [b(this)["val"](), f])
                })["appendTo"]("#" + g);
                b("#" + g)["data"]("editor", this)
            };
            this["destroy"] = function () {
                b("#" + g + " input")["each"](function () {
                    c[d]["value"] = b(this)["val"]();
                    b(e)["trigger"]("textchange", [b(this)["val"](), f])
                })
            }
        }
    };
    a.init = function (l, k) {
        var j = b(window)["width"](),
		i = b(window)["height"](),
		r = Raphael(l, j * 1.5, i * 1.5),
		h = {},
		g = {};
        b.extend(true, a.config, k);
        b(document)["keydown"](function (D) {
            if (!a.config["editable"]) {
                return
            }
            if (D.keyCode == 46) {
                var C = b(r)["data"]("currNode");
                if (C) {
                    if (C.getId()["substring"](0, 4) == "rect") {
                        b(r)["trigger"]("removerect", C)
                    } else {
                        if (C.getId()["substring"](0, 4) == "path") {
                            b(r)["trigger"]("removepath", C)
                        }
                    }
                    b(r)["removeData"]("currNode")
                }
            }
        });
        var B = a.config["props"]["props"];
        if (k.restore) {
            var A = k.restore;
            if (A.props) {
                B = b.extend(true, {}, a.config["props"]["props"], A.props["props"])
            }
        }
        b(document)["click"](function () {
            b(r)["data"]("currNode", null);
            b(r)["trigger"]("click", {
                getId: function () {
                    return "00000000"
                }
            });
            b(r)["trigger"]("showprops", [B, {
                getId: function () {
                    return "00000000"
                }
            }
				])
        });
        function y() {
            var C = 0;
            for (t in h) {
                C++
            }
            return C
        }
        function w() {
            var C = 0;
            for (t in g) {
                C++
            }
            return C
        }
        var v = function (D, C) {
            if (!a.config["editable"]) {
                return
            }
            if (C.getId()["substring"](0, 4) == "rect") {
                h[C.getId()] = null;
                if (a.config["removeRectHandler"]) {
                    a.config["removeRectHandler"](C.uiobject()["props"]["Id"]["value"])
                }
                C.remove()
            } else {
                if (C.getId()["substring"](0, 4) == "path") {
                    g[C.getId()] = null;
                    C.remove();
                    if (a.config["removePathHandler"]) {
                        a.config["removePathHandler"](C.uiobject()["props"]["Id"]["value"])
                    }
                }
            }
        };
        b(r)["bind"]("removepath", v);
        b(r)["bind"]("removerect", v);
        b(r)["bind"]("addrect", function (G, I, D) {
            function newGuid() {
                var guid = "";
                for (var i = 1; i <= 32; i++) {
                    var n = Math.floor(Math.random() * 16.0).toString(16);
                    guid += n;
                    if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                        guid += "";
                }
                return guid;
            }
            if (ValidateRect != undefined && !ValidateRect(I, h)) {
                return
            }
            var F = b.extend(true, {}, a.config["tools"]["states"][I], D);
            var E;
            if (F.props["Id"] && B.Id) {
                var C = newGuid();
                E = B.Id["value"] + "." + I + "_" + C;
                while (a.config["restore"]["states"][E]) {
                    C++;
                    E = B.Id["value"] + "." + I + "_" + C
                }
                F.props["Id"]["value"] = E;
                F.props["Name"]["value"] = I + "_" + C;
                if (a.config["tools"]["states"][I]["showText"]) {
                    a.config["tools"]["states"][I]["showText"](F, I, C)
                }
            }
            if (a.config["addRectHandler"]) {
                a.config["addRectHandler"](F)
            }
            var H = new a.rect(F, r);
            h[H.getId()] = H;
            if (E && a.config["tools"]["states"][I]["callback"]) {
                a.config["tools"]["states"][I]["callback"](E)
            }
        });
        var m = function (F, G, E) {
            if (ValidatePath != undefined && !ValidatePath(G, E, h, g)) {
                return
            }
            var H = new a.path({}, r, G, E);
            g[H.getId()] = H;

            function newGuid() {
                var guid = "";
                for (var i = 1; i <= 32; i++) {
                    var n = Math.floor(Math.random() * 16.0).toString(16);
                    guid += n;
                    if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                        guid += "";
                }
                return guid;
            }
            if (a.config["path"]["props"]["Id"] && B.Id) {
                var D = B.Id["value"] + "." + a.config["path"]["props"]["Id"]["value"] + "_" + newGuid();
            }
            var I = H.uiobject();
            I.props["Id"]["value"] = D;
            I.props["Name"]["value"] = a.config["path"]["props"]["Id"]["value"] + "_" + newGuid();
            if (a.config["addPathHandler"]) {
                a.config["addPathHandler"](I)
            }
        };
        b(r)["bind"]("addpath", m);
        b(r)["data"]("mod", "point");
        if (a.config["editable"]) {
            b("#myflow_tools .node")["hover"](function () {
                b(this)["addClass"]("mover")
            }, function () {
                b(this)["removeClass"]("mover")
            });
            b("#myflow_tools .selectable")["click"](function () {
                b(".selected")["removeClass"]("selected");
                b(this)["addClass"]("selected");
                b(r)["data"]("mod", this["id"])
            });
            b("#myflow_tools .state")["each"](function () {
                b(this)["draggable"]({
                    revert: true,
                    deltaX: 10,
                    deltaY: 10,
                    proxy: function (C) {
                        var D = b("\x3Cdiv class=\x22proxy\x22\x3Epoxy\x3C/div\x3E");
                        D.html(b(C)["html"]())["appendTo"]("body");
                        return D
                    }
                })
            });
            b("#myflow_tools")["draggable"]({
                handle: "#myflow_tools_handle"
            })["css"](a.config["tools"]["attr"]);
            b(l)["droppable"]({
                onDragEnter: function (D, C) {
                    b(C)["draggable"]("options")["cursor"] = "auto"
                },
                onDragLeave: function (D, C) {
                    b(C)["draggable"]("options")["cursor"] = "not-allowed"
                },
                onDrop: function (D, C) {
                    if (b(C)["hasClass"]("panel")) {
                        return
                    }
                    b(r)["trigger"]("addrect", [b(C)["attr"]("type"), {
                        attr: {
                            x: b(".proxy")["offset"]()["left"] - 100,
                            y: b(".proxy")["offset"]()["top"] - 10
                        }
                    }
						])
                }
            });
            function x() {
                var C = "{states:{";
                for (var D in h) {
                    if (h[D]) {
                        C += h[D]["getId"]() + ":" + h[D]["toJson"]() + ","
                    }
                }
                if (C.substring(C.length - 1, C.length) == ",") {
                    C = C.substring(0, C.length - 1)
                }
                C += "},paths:{";
                for (var D in g) {
                    if (g[D]) {
                        C += g[D]["getId"]() + ":" + g[D]["toJson"]() + ","
                    }
                }
                if (C.substring(C.length - 1, C.length) == ",") {
                    C = C.substring(0, C.length - 1)
                }
                C += "},props:{props:{";
                for (var D in B) {
                    C += D + ":{value:\x27" + B[D]["value"] + "\x27},"
                }
                if (C.substring(C.length - 1, C.length) == ",") {
                    C = C.substring(0, C.length - 1)
                }
                C += "}}}";
                return C
            }
            b("#myflow_save")["click"](function () {
                var C = x();
                a.config["tools"]["save"]["onclick"](C)
            });
            b("#myflow_xml")["click"](function () {
                var C = x();
                a.config["tools"]["xml"]["onclick"](C)
            });
            new a.props({}, r)
        }
        if (k.restore) {
            var s = k.restore;
            var o = {};
            if (s.states) {
                for (var f in s.states) {
                    var q = new a.rect(b.extend(true, {}, a.config["tools"]["states"][s.states[f]["uitype"] ? s.states[f]["uitype"] : s.states[f]["type"]], s.states[f]), r);
                    q.restore(s.states[f]);
                    o[f] = q;
                    h[q.getId()] = q
                }
            }
            if (s.paths) {
                for (var f in s.paths) {
                    var u = new a.path(b.extend(true, {}, a.config["tools"]["path"], s.paths[f]), r, o[s.paths[f]["from"]], o[s.paths[f]["to"]]);
                    u.restore(s.paths[f]);
                    g[u.getId()] = u
                }
            }
        }
        var n = a.config["historyRects"],
		e = a.config["activeRects"];
        if (n.rects["length"] || e.rects["length"]) {
            var t = {},
			o = {},
			d;
            for (var z in g) {
                id = g[z]["from"]()["uiobject"]()["props"]["Id"]["value"];
                if (!o[id]) {
                    o[id] = {
                        rect: g[z]["from"](),
                        paths: {}

                    }
                }
                o[id]["paths"][g[z]["uiobject"]()["props"]["Id"]["value"]] = g[z];
                id = g[z]["to"]()["uiobject"]()["props"]["Id"]["value"];
                if (!o[id]) {
                    o[id] = {
                        rect: g[z]["to"](),
                        paths: {}

                    }
                }
            }
            for (var p = 0; p < n.rects["length"]; p++) {
                if (o[n.rects[p]["id"]]) {
                    o[n.rects[p]["id"]]["rect"]["attr"](n.rectAttr)
                }
                for (var c = 0; c < n.rects[p]["paths"]["length"]; c++) {
                    if (o[n.rects[p]["id"]]["paths"][n.rects[p]["paths"][c]]) {
                        o[n.rects[p]["id"]]["paths"][n.rects[p]["paths"][c]]["attr"](n.pathAttr)
                    }
                }
            }
            for (var p = 0; p < e.rects["length"]; p++) {
                if (o[e.rects[p]["id"]]) {
                    o[e.rects[p]["id"]]["rect"]["attr"](e.rectAttr)
                }
                for (var c = 0; c < e.rects[p]["paths"]["length"]; c++) {
                    if (o[e.rects[p]["id"]]["paths"][e.rects[p]["paths"][c]]) {
                        o[e.rects[p]["id"]]["paths"][e.rects[p]["paths"][c]]["attr"](e.pathAttr)
                    }
                }
            }
        }
    };
    b.fn["myflow"] = function (c) {
        return this["each"](function () {
            a.init(this, c)
        })
    };
    b.myflow = a
})(jQuery);
