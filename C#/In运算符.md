### Python
```
Class Example(object):

  def __init__(self):
    self.example = [1,2]
    
  def __contains__(self, item):
    return item in self.example
    
    
a = Example()
print 1 in a

output:true
```

### C#
对应的是对象的Contains方法
