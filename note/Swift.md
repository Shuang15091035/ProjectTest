###元组
元组：可以存储相关数据变量的对象,可以把元组理解为一种只能存放数据，却没有定义方法的轻量级数据结构。(元组是一个值类型)	
1.元组的创建	
var newTuple  = ("ls",26)	
2.1.元组直接解绑	
var (name,age) = newTuple	
print("name = \(name),age = \(age)")	
2.2过滤元素解绑(用_过滤不需要的元素)	
var (name,_) = newTuple	
print("name =  \(name)")	
2.3元组按下标解绑	
print(newTuple.0,newTuple.1)	
2.4变量名解绑	
var newTuple1 = (name:"ls",age:32)	
print(newTuple1.name,newTuple1.age)	

###字符串的的使用
1.字符串的创建
var emptyString = ""	
var emptyString2 = String()	
if emptyString.isEmpty{	
		
}	

###可选类型和断言
可选类型其根源是一个枚举类型，内部有None 和Some两种类型，其中的nil就是Optional.None,非nil就是Optional.Some,然后通过Some包装(Wrap)原始值，这是为什么使用Optional要拆包（从enum中取出原始值）的原因，也是PlayGround会把Optional值显示为类似{Some"hello world"}的原因，这是enum Optional的定义：
1.问号？	
a.声明时添加？，告诉编译器这个是Optional的，如果声明时没有手动初始化，就自动初始化为nil	
b.在对变量值操作前添加？，判断如果变量时nil，则不响应后面的方法。	
2.叹号！	
a.声明时添加！，告诉编译器这个是Optional的，并且之后对该变量操作的时候，都隐式的在操作前添加！	
b.在对变量操作前添加！，表示默认为非nil，直接解包进行处理	
断言	
assert（）条件不满足时触发，有助于定位和排除bug	

###添加新数据