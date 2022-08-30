namespace Game;

public static class Utils
{
    //esto es para el reflection. devuelve un array de tipos que implementan una interfaz o que heredan de una clase
   public static Type[] TypesofEverything<T>()
    {
        return typeof(T).Assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

}
