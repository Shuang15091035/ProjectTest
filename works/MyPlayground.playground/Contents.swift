//: Playground - noun: a place where people can play

import UIKit



//var newType = ("kt",20)
//
//var (name,age) = newType
//print("name = \(name)")
//
//print(age)
//
//print(newType.1,newType.0)
//
//var type = (name:"kt", age:20)
//
//print(type.name);

//let person = ("liu", 12, 1.75)
//
//let (name, age, height) = person
//
//let (_, good, _) = person
//
//
//print(good)
//
//print(person.0)
////#pramark 元组为值类型
//func getHeightAndAge() -> (Int, Float)? {
//    var age:Float
//    let height1 = 12
//    age = 12.0
//    return(height1,age)
//}


//var optionalName: String? = nil
//var greeting = "Hello!"
//if let name = optionalName {
//    greeting = "Hello, \(name)"
//}

//var optional:String? = "Shuang"
//
//if let name = optional {
//    print(name)
//}


//var newTuple1 = (name:"ls",age:32)
//print(newTuple1.name,newTuple1.age)

//
//func person33(start s: String, end: Int) -> (String,Int){
//    return (s,end)
//}
//
//person33(start: "56", end: 12)
//print(person33(start: "Shuang" ,end: 15))

//\()相当于OC中%@ 内插的表达式中不能直接包含双引号、单引号或者反斜杠
//let combine1 = "\u{1112}"
//let combine2 = "\u{1112}\u{1161}"
//let combine3 = "\u{1112}\u{1161}\u{11AB}"
//
//print("combine1 = \(combine1)")
//print("combine2 = \(combine2)")
//print("combine3 = \(combine3)")
//
//var combine = String();
//var combine4 = "good"
//
//combine += combine4// 字符串组合
//
//var char:Character = "a"
//combine.append(char);
//
//let stringValue1 = "hello, world"
//let stringValue2 = "\u{65}\u{301}"
////计算字符串长度方法
//stringValue1.lengthOfBytesUsingEncoding(NSUTF8StringEncoding)
//(stringValue1 as NSString).length
//
//print("string2 = \(stringValue2)")
//
//var number = 3
//var time = 2
//var setence = "\(number)的\(time)倍是\(number * time)"
//
//if number == time{
//    print("结果正确")
//}else{
//    print("失败")
//}
//
////比较特殊的一点是，swift在比较两个字符串时，并不会逐一比较每个Unicode标量是否相等，而是会根据字符串的实际语义来比较。所以在下面的例子中，比较结果需要特别留心一下
//var compare1 = "caf\u{E9}" //\u{E9}是带声调的e，形如é
//var compare2 = "caf\u{65}\u{301}" //这是字母e加上声调合成的é
//if compare1 == compare2{
//    //它们依然是相等的
//    print("\(compare1) is equal to \(compare2)")
//}
//
//var compare3 = "\u{41}" //拉丁字母A
//var compare4 = "\u{0410}" //斯拉夫字母A
//if compare3 != compare4{
//    //虽然表现相同，但实际语义不同，所以字符串依然不同
//    print("\(compare3) is not equal to \(compare4)")
//}
//
////字符串前后缀
//var prefixAndSuffix = "hello world"
//if prefixAndSuffix.hasSuffix("h"){
//    
//}
//if prefixAndSuffix.hasPrefix("h"){
//    
//}
//if prefixAndSuffix.hasSuffix("orld"){
//    
//}
//
//var a = 10
//
//let b = 20.2;
//
//var c = Double(a) + b
//
//typealias name = Int
//
//
//let goodName:name = 10
//
//print("\(goodName)")
//
//
////可选类型
//
//var string = "564"
//
//let double = (string as NSString).doubleValue
//let result = 2.3
//print("\(double + result)")
//print("")

//将字符串转化为整形值(可选类型，强制解封)
//var string = "456"
//var string1 = "45a"
//
//let unknownValue:Int? = 3
////let unknownValue:Int? = nil
//if var variable = unknownValue {
//    print("variable = \(variable)")
//}
//
//var a = 25
//
//assert(a > 10,"gongxiwancheng")
//
//
//var arrayOne:Array<Int> = [1,2,3]
//print("arrayLong = \(arrayOne[0])")
//
//var arrayTwo:[String] = ["1","2","3"]
//print("arrayTwo = \(arrayTwo[2])")
//
//var typeTuple = (1,2,3)//元组
//print(typeTuple.0)
//var (name,age,height) = typeTuple
//print(name,age,height)
//

//var arrayThree = [1,2,3]//数组
//print("arrayThree = \(arrayThree)")
//
//var arrayMixed = [1,"abc",true,1.5]
//print("arrayMixed = \(arrayMixed)")
//arrayMixed.append("good")
//arrayMixed.removeAtIndex(2)
//
////方法一，使用数组的append函数
//var arrThree = [1,2,3]
//arrayThree.append(1)//新添加的数组中的数据和原数组中的数据相同，否则编译出错
//print("arrayThree = \(arrayThree)")
//
////方法二，使用加法运算符
//var arrayThrees = [1,2,3]
//arrayThree += [4,5,6,7]
//arrayThree.insert(15, atIndex: 2)
//print("arrayThree = \(arrayThree)")
//
//var ArrayThree = [1,2,3]
//var numberThree = arrayThree.removeAtIndex(2)
//var numberTwo = arrayThree.removeLast()
//
//var secondValue = 56
//arrayThree[2] = [secondValue,56,12]

//var arrayThree = [1,2,3]
//var firstNumber = 5
//var secondNumber = 26
//arrayThree[0...1] = [firstNumber,secondNumber]
//for number in arrayThree{
//    print(number)
//}

//方法二：使用enumerate函数
//var arrayThree = [Int](count: 5, repeatedValue: 0)
//var arrayFour = Array<Int>(count: 5, repeatedValue: 0)
//var arrayFiver = Array(count: 5, repeatedValue: 0)
//print("第三个数组为：\(arrayThree)")
//print("第四个数组为：\(arrayFour)")
//print("第五个数组为：\(arrayFiver)")


//字典的使用
//var dictionary: Dictionary<String,Int> = ["key":21]
//var dictionary2:[String:Int] = ["key2":2,"key3":3]
//print("dictionary: \(dictionary.count)")
//dictionary["key"] = 20
//
//if !dictionary.isEmpty{
//    
//}

//var dictionary:Dictionary<String,String> = ["key1":"56","key2":"89"]
//print(dictionary.values)
//dictionary.updateValue("10", forKey: "key1")
//print(dictionary)
//
//for(keyName,valueName) in dictionary{
//    print("keyName:\(keyName) valueName = \(valueName)")
//}
//
//var array:Array<String> = ["string","10"]

//var btn:UIButton
//
//var btn2:UIButton = UIButton()
//
//btn2.backgroundColor = UIColor.whiteColor()
//btn2.imageEdgeInsets = UIEdgeInsets(top:1,left:2,bottom:3,right:4)
//btn2.frame = CGRectMake(0, 0, 0, 0)
//btn2.setTitle("按钮", forState: UIControlState.Reserved)
//
//struct Person {
//    
//    var name:String!
//    
//}
//
//var mike:Person = Person()
//mike.name = "Mike"
//
//var jake = mike
//jake.name = "Jake"
//
//print("\(mike.name) and \(jake.name)")

