
using System;

namespace TestFantasyGame.Source.Tools;


public class Command<R, P> { 
    

    protected Func<P,R> ExecuteFunc {get; set;}   
    public Command (Func<P,R> func){
        ExecuteFunc = func;
    }

    public virtual R ExecuteCommand (P param){
        return ExecuteFunc(param);
    }
}
