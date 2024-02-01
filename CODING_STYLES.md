# Coding Styles

기본적인 코딩 스타일은 C# 기본 스타일을 따릅니다. [Unity 코딩 스타일 문서](https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity)를 참고할 수 있습니다.


## Casing Schemes

### 1. Camel Case

단어와 단어 사이를 띄어쓰기 없이 단어 첫 글자를 대문자로 표기해 구분합니다. 첫 단어의 첫 문자는 소문자로 시작합니다.

`game object` -> `gameObject`

`uppercase converter` -> `uppercaseConverter`

### 2. Pascal Case

단어와 단어 사이를 띄어쓰기 없이 단어 첫 글자를 대문자로 표기해 구분합니다. 첫 단어의 첫 문자는 대문자로 시작합니다.

`game object` -> `GameObject`

`uppercase converter` -> `UppercaseConverter`

### 3. Snake Case

단어와 단어 사이의 띄어쓰기를 언더바('_')로 표기해 구분합니다. 일반적으로 각 단어는 대소문자 중 한 종류로 통일합니다.

`game object` -> `game_object`

`uppercase converter` -> `UPPERCASE_CONVERTER`

## 필드와 변수

- 변수 이름과 필드에는 명사를 사용합니다.

- `bool` 타입에 한해서 `is-`나 `has-` 등의 접두사로 의문문을 만들어 이름을 짓습니다.
    - `isEmpty`, `isRunning` 등이 예시입니다.

- 알려진 수학 상수나 약어를 제외하고, 충분히 설명이 되는 명사로 이름을 짓습니다.
    - `a`가 아닌 `playerName` 등이 예시입니다.

- 클래스나 구조체의 필드라면 중복되는 수식어구를 생략하는 것을 고려할 수 있습니다.
    - `Player`의 클래스에 `playerName` 필드가 존재한다면, `playerName` 필드를 `name` 필드로 이름을 바꿔 수식어구를 생략할 수 있습니다.

- public 필드에는 `PascalCase`, private 필드에는 `camelCase`에 접두사 `_`를 붙여 활용합니다.
    -  protected, internal, protected internal 필드 등에는 관례적으로 접두사가 없는 `camelCase`를 활용합니다.

- public, protected, private 등의 접근지정자를 명시합니다.
    - 클래스의 필드나 메소드의 기본 접근지정자는 internal입니다.

- 변수나 필드가 배열이나 `System.Collection` 하위 타입이라면 복수 형태로 이름을 짓는 것을 고려할 수 있습니다.
    - 이름이 중복된다든가 부득이하게 이름을 복수 형태로 지을 수 없다면 끝에 컬렉션 종류를 명시할 수 있습니다.
    - data 등 관례적으로 복수 형태를 기본으로 사용하는 명사는 컬렉션 종류를 명시할 수 있습니다.

## 열거형(Enums)

열거형 타입의 상숫값은 `PascalCase`로 표기됩니다. 열거형 타입의 이름은 단수 형태를 활용하지만, 예외로 비트플래그 열거형 타입은 복수 형태를 타입 이름에 활용합니다.

## 클래스와 인터페이스

- 클래스와 인터페이스 이름은 `PascalCase`로 표기합니다.

- 인터페이스는 접두사로 `I`를 붙입니다.
    - `Movable` 인터페이스는 `IMovable`로 이름짓는 것을 권장합니다.

## 메서드

- 메서드는 `PascalCase`로 이름짓습니다.

- `bool` 타입을 반환하는 메서드에 한해서 `is-`나 `has-` 등의 접두사로 의문문을 만들어 이름을 짓습니다.

## 이벤트 핸들러

- 이벤트 발생 시 동작을 처리할 이벤트 핸들러는 문장 형태의 메서드 이름과 `On-` 접두사로 메서드의 이름을 짓습니다.
    - `ReceivingMessage`의 이벤트 핸들러 메서드는 `OnReceivingMessage`나 `OnMessageReceived` 등의 이름을 권장합니다.

## 네임스페이스(Namespaces)

- 네임스페이스는 `PascalCase`로 이름짓습니다.
