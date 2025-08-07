// -------------------------------------------------------- 
// -------------------- CODE GEN WARNING ------------------ 
// This file was created by a code generator.  You may edit 
// it but be aware that your changes may disappear suddenly 
// when the generator program runs next!                    
// -------------------------------------------------------- 


using drewCo.Web;

class LoginForm 
{

  public string Username { get; set; } = "Dave";
  public string Password { get; set; } = "*";
  public bool IsWorking { get; set; } = false;

  public string CreateDOM()
  {
    var root = new HTMLNode("div");
    var val = getValue();
    root.SetAttribute("class", val);


    var node = new HTMLNode("div");
    node.SetAttribute("class", "inputs");


    var node1 = new HTMLNode("label");
    node1.SetAttribute("for", "username");
    var textNode = HTMLNode.CreateTextNode("Username");
    node.AddChild(node1);


    var node2 = new HTMLNode("input");
    node2.SetAttribute("type", "text");
    node2.SetAttribute("name", "username");
    node.AddChild(node2);
    root.AddChild(node);


    var node3 = new HTMLNode("div");
    node3.SetAttribute("class", "actions");


    var node4 = new HTMLNode("button");
    node4.SetAttribute("class", "btn btn-login");
    var textNode1 = HTMLNode.CreateTextNode("Log In");
    node3.AddChild(node4);
    root.AddChild(node3);

    // Set property values on attributes so that 'Bind' calls will work.
    var val1 = this.Username ?? string.Empty;
    if (val1 != string.Empty)
    {
      root.SetAttribute("data-id-Username", val1);
    }
    var val2 = this.Password ?? string.Empty;
    if (val2 != string.Empty)
    {
      root.SetAttribute("data-id-Password", val2);
    }
    var val3 = this.IsWorking.ToString() ?? string.Empty;
    if (val3 != string.Empty)
    {
      root.SetAttribute("data-id-IsWorking", val3);
    }

    string res = root.ToHTMLString();
    return res;
  }

  string getValue()
  {
    return ("login-form" + IsWorking).ToString();
  }

}
