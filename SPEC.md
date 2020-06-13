# Language Specification #

⚠️ This repo is WIP and subject to changes

## Keywords ##

### The `@` character ###

In *ASDML*, all keywords must be prefixed by the `@` character.

### Null literal ###

Null means something has no valid value. In ASDML, the null keyword is `@null`.

### Logical literals ###

There are two logical keywords: `@true` and `@false`.

## Primitive Literals ##

### Logical ###

See logical literals.

### Number ###

Numbers are character sequences matching `^[\+-]?\d+(\.\d+)?([Ee][\+-]\d+)?$` regular expression. Try it on [RegExr](https://regexr.com/56lm8).

### Text ###

Text literals can be written between quotation marks: `"Hello"`.

Quotation marks can be omitted if the literal:

* does not contain whitespace character
* does not begin with the following characters: `@#`
* cannot be interpreted as a number literal

## Basic Syntax ##

In ASDML, *group*s refer to objects or classes in programming languages. By default, curly brackets follow the group name.

``` ASDML
Group {

}
```

Classes have properties. Properties have values. In ASDML, properties start with a period character. Values follow property names after a whitespace character.

``` ASDML
Group {
    .StringProperty "Value"
}
```

Some objects can have children (for example, a GUI window, arrays, lists, IEnumerable in C#, Iterable in Java).ssssss

Groups can have child groups. Child groups have no prefix. A group can have arbitrary number of child groups.

``` ASDML
Group {
    ChildGroup1 {

    }
    ChildGroup2 {

    }
}
```
