# Chapter 6 - Testing

Unit tests give 2 main advantages:
1. Better understanding of the code.
2. Great confidence to make changes.

Both of these advantages apply to your own code just as much as others’ code.

## 6.1. Unit Testing async Methods
You have an async method that you need to unit test.

Most modern unit test frameworks support async Task unit test methods, including MSTest, NUnit, and xUnit.

If your unit test framework does not support async Task unit tests, then it will need some help to wait for the asynchronous operation under test. One option is to use Task.Wait and unwrap the AggregateException if there is an error. 

## 6.2. Unit Testing async Methods Expected to Fail
You need to write a unit test that checks for a specific failure of an async Task method.

MSTest does support failure testing via the regular ExpectedExceptionAttribute.

## 6.3. Unit Testing async void Methods
You have an async void method that you need to unit test.

Stop.
You should do your dead-level best to avoid this problem rather than solve it. If it is possible to change your async void method to an async Task method, then do so.

If it’s completely impossible to change your method and you absolutely must unit test an async void method, then there is a way to do it. You can use the AsyncContext class from the Nito.AsyncEx library:

```C#
// Not a recommended solution; see above.
[TestMethod]
public void MyMethodAsync_DoesNotThrow()
{
    AsyncContext.Run(() =>
    {
        var objectUnderTest = ...;
        objectUnderTest.MyMethodAsync();
    });
}
```