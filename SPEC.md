# Language Specification #

⚠️ This repo is WIP and subject to changes

## Keywords and primitive literals ##

### The `@` character ###

In ASDML, all keywords must be prefixed by the `@` character.

### Null literal ###

Null means something has no valid value. In ASDML, the null keyword is `@null`.

### Logical literals ###

There are two logical keywords: `@true` and `@false`.

### Number literals ###

Numbers are character sequences matching `^[\+-]?\d+(\.\d+)?([Ee][\+-]\d+)?$` regular expression. Try it on [RegExr](https://regexr.com/56lm8). This means:

* An optional `+` or `-` sign, at most one
* 1 or more digits
* Optional
  * Decimal point
  * 1 or more digits
* Optional (the previous number multiplied by a positive power of 10)
  * Letter `E` or `e`
  * Exactly 1 `+` or `-` sign
  * 1 or more digits

### Text literals ###

Text literals can be written between quotation marks: `"Hello"`.

Quotation marks can be omitted if the literal:

* does not contain whitespace character
* does not begin with the following characters: `@#`
* cannot be interpreted as a number literal

## Syntax ##

### Groups ###

In ASDML, *group*s refer to objects or classes in programming languages. By default, curly brackets follow the group name. Let's create an empty window class:

``` csharp
class Window {
}
```

This class looks like this in ASDML:

``` asdml
Window {
}
```

### Primitive properties ###

Classes have properties. Properties have values. Let's add some properties to the window class:

``` csharp
class Window {
  int Width;
  int Height;
}

Window window = new Window();
window.Width = 800;
window.Height = 600;
```

In ASDML, properties start with a period character. Values follow property names after a whitespace character. The `window` object looks like this in ASDML:

``` asdml
Window {
  .Width 800
  .Height 600
}
```

### Non-primitive properties ###

Classes can have non-primitive properties. Let's add an OK button:

```csharp
class Window {
  int Width;
  int Height;
  Button OkButton;
}
class Button {
  string Text;
}

Window window = new Window();
window.Width = 800;
window.Height = 600;
Button button = new Button();
button.Text = "Click me";
window.OkButton = button;
```

Non-primitive properties are like primitives, but in place of the primitive value, a group is written:

``` asdml
Window {
  .Width 800
  .Height 600
  .OkButton Button {
    .Text "Click me"
  }
}
```

### Nested groups ###

Some objects can have children or items (for example, a GUI window, arrays, lists, IEnumerable in C#, Iterable in Java).

In ASDML, groups can have nested groups. Nested groups have no prefix. A group can have arbitrary number of nested groups. Let's add some controls to the window:

```csharp
class Window {
  int Width;
  int Height;
  void Add(Control control);
}
interface Control {
}
class TextBox : Control {
  string Text;
}
class Button : Control {
  string Text;
}

Window window = new Window();
window.Width = 800;
window.Height = 600;
TextBox textBox = new TextBox();
textBox.Text = "Hello";
window.Add(textBox);
Button button = new Button();
button.Text = "Click me";
window.Add(button);
```

For example, a window containing a `TextBox` and a `Button` looks like this in ASDML:

``` asdml
Window {
  .Width 800
  .Height 600
  TextBox {
    .Text Hello
  }
  Button {
    .Text "Click me"
  }
}
```
