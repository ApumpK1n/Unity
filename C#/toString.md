## 将对象变成字符串类型
### Python
```
class CStr(object)
    def __init__(self, string):
        self.m_string = string
        
    def __str__(self):
        return "I am %s" % self.m_string
   
eg:
oStr = CStr("PumpK1n")
print oStr
output: "I am PumpK1n"
```

### CSharp
```
class CStr
{
    private string Name;
    public CStr(string s)
    {
        Name = s;
    }
    
    public override String ToString()
    {
        return "I am" + Name;
    }
}
```
