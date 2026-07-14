// -------------------------------------------------------- 
// -------------------- CODE GEN WARNING ------------------ 
// This file was created by a code generator.  You may edit 
// it but be aware that your changes may disappear suddenly 
// when the generator program runs next!                    
// -------------------------------------------------------- 

class LoginForm {

  _Username: string = "Dave";
  _Password: string = "*";
  _IsWorking: boolean = false;


  // ---- DOM Elements ------
  _Root: HTMLElement;
  _Node: HTMLElement;
  _Node1: HTMLElement;
  _Node2: HTMLElement;
  _Node3: HTMLElement;
  _Node4: HTMLElement;

  CreateDOM(): HTMLElement {
    this._Root = document.createElement('div');
    var val = this.getValue();
    this._Root.setAttribute("class", val);


    this._Node = document.createElement('div');
    this._Node.setAttribute("class", "inputs");


    this._Node1 = document.createElement('label');
    this._Node1.setAttribute("for", "username");
    this._Node1.insertAdjacentText('beforeend', 'Username');
    this._Node.append(this._Node1);


    this._Node2 = document.createElement('input');
    this._Node2.setAttribute("type", "text");
    this._Node2.setAttribute("name", "username");
    var val1 = this.getValue1();
    this._Node2.setAttribute("value", val1);
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
  }



  getValue(): string{
    return ("login-form" + this.IsWorking).toString();
  }

  getValue1(): string{
    return (this.Username).toString();
  }

  Bind(dom:HTMLElement) {
    // NOTE: A correctly formed DOM for this type is assumed!
    this._Root = <HTMLElement>dom;
    this._Node = <HTMLElement>dom.children[0];
    this._Node1 = <HTMLElement>dom.children[0].children[0];
    this._Node2 = <HTMLElement>dom.children[0].children[1];
    this._Node3 = <HTMLElement>dom.children[1];
    this._Node4 = <HTMLElement>dom.children[1].children[0];

    // All property values must be included on the root node if you want to bind them.
    const val = this._Root.getAttribute('data-id-Username');
    if (val != undefined){
      this.Username = val;
    }

    this._Root.removeAttribute('data-id-Username');
    const val1 = this._Root.getAttribute('data-id-Password');
    if (val1 != undefined){
      this.Password = val1;
    }

    this._Root.removeAttribute('data-id-Password');
    const val2 = this._Root.getAttribute('data-id-IsWorking');
    if (val2 != undefined){
      this.IsWorking = val2 == 'true';
    }

    this._Root.removeAttribute('data-id-IsWorking');
  }
  public get Username() {
    return this._Username;
  }

  public set Username(username_: string) {
    this._Username = username_;
    const val0 = this.getValue1();
    this._Node2.setAttribute('value', val0);
  }

  public get Password() {
    return this._Password;
  }

  public set Password(password_: string) {
    this._Password = password_;
  }

  public get IsWorking() {
    return this._IsWorking;
  }

  public set IsWorking(isWorking_: boolean) {
    this._IsWorking = isWorking_;
    const val0 = this.getValue();
    this._Root.setAttribute('class', val0);
  }

}
