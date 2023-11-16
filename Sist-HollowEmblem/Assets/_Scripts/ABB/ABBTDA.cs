public interface ABBTDA
{
    int Raiz();
    NodoABB HijoIzq();
    NodoABB HijoDer();
    bool ArbolVacio();
    void InicializarArbol();
    void AgregarElem(ref NodoABB n, NodoABB nodo);
    void EliminarElem(ref NodoABB n, int x);
}
