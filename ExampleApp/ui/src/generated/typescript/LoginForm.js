// -------------------------------------------------------- 
// -------------------- CODE GEN WARNING ------------------ 
// This file was created by a code generator.  You may edit 
// it but be aware that your changes may disappear suddenly 
// when the generator program runs next!                    
// -------------------------------------------------------- 
var LoginForm = /** @class */ (function () {
    function LoginForm() {
        this._Username = "Dave";
        this._Password = "*";
        this._State = "";
    }
    LoginForm.prototype.CreateDOM = function () {
        this._Root = document.createElement('div');
        this._Root.setAttribute("class", "login-form");
        var val = this.getValue();
        this._Root.setAttribute("data-state", val);
        this._Node = document.createElement('div');
        this._Node.setAttribute("class", "inputs");
        this._Node1 = document.createElement('label');
        this._Node1.setAttribute("for", "username");
        this._Node1.insertAdjacentText('beforeend', 'Username');
        this._Node.append(this._Node1);
        this._Node2 = document.createElement('input');
        this._Node2.setAttribute("type", "text");
        this._Node2.setAttribute("name", "username");
        this._Node.append(this._Node2);
        this._Root.append(this._Node);
        this._Node3 = document.createElement('div');
        this._Node3.setAttribute("class", "actions");
        this._Node4 = document.createElement('button');
        this._Node4.setAttribute("class", "btn btn-login");
        this._Node4.insertAdjacentText('beforeend', 'Log In');
        this._Node3.append(this._Node4);
        this._Root.append(this._Node3);
        return this._Root;
    };
    LoginForm.prototype.getValue = function () {
        return (this.State).toString();
    };
    LoginForm.prototype.Bind = function (dom) {
        // NOTE: A correctly formed DOM for this type is assumed!
        this._Root = dom;
        this._Node = dom.children[0];
        this._Node1 = dom.children[0].children[0];
        this._Node2 = dom.children[0].children[1];
        this._Node3 = dom.children[1];
        this._Node4 = dom.children[1].children[0];
        // All property values must be included on the root node if you want to bind them.
        var val = this._Root.getAttribute('data-id-Username');
        if (val != undefined) {
            this.Username = val;
        }
        this._Root.removeAttribute('data-id-Username');
        var val1 = this._Root.getAttribute('data-id-Password');
        if (val1 != undefined) {
            this.Password = val1;
        }
        this._Root.removeAttribute('data-id-Password');
        var val2 = this._Root.getAttribute('data-id-State');
        if (val2 != undefined) {
            this.State = val2;
        }
        this._Root.removeAttribute('data-id-State');
    };
    Object.defineProperty(LoginForm.prototype, "Username", {
        get: function () {
            return this._Username;
        },
        set: function (username_) {
            this._Username = username_;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(LoginForm.prototype, "Password", {
        get: function () {
            return this._Password;
        },
        set: function (password_) {
            this._Password = password_;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(LoginForm.prototype, "State", {
        get: function () {
            return this._State;
        },
        set: function (state_) {
            this._State = state_;
            var val0 = this.getValue();
            this._Root.setAttribute('data-state', val0);
        },
        enumerable: false,
        configurable: true
    });
    return LoginForm;
}());
//# sourceMappingURL=LoginForm.js.map