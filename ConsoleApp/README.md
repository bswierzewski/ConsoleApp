Program.cs

1. Usingi w projekcie powinny znajdować się przezd namespace oraz proponuję usunąć niepotrzebne, zmniejsza to ilość lini w pliku i poprawia czytelność kodu
2. Błąd w nazwie pliku "dataa.csv" -> w projekcie nie istnieje plik o takiej nazwie
3. Nazwa pliku powinna być przekazywana z zewnątrz np z args. Jeśli jednak ma zostać na szytwno w kodzie to porponuję usunąć string[] args z metody
4. DataReader wykonuje tylko jedną metodę i nie przypisuje żadnej zmiennej to można skrócić zapis do new DataReader().ImportAndPrintData("data.csv");
5. Nie widzę żadnej obsługi błędów co nie jest dobrą praktyką gdyż program przestanie działać przy pierwszym błędzie

DataReader.cs

1. Usingi przed namespace, niepotrzebne do usunięcia
2. IEnumerable<ImportedObject> ImportedObjects;
    a. Wygląda na zmienną prywatną jednak napisana jest z dużej litery => albo poprawić nazwę albo dodać {get;set;}
    b. Dobrą praktyką jest inicjalizacja kolejki by uniknąć null reference
3. Metoda ImportAndPrintData jak nazwa wskazuje wykonuje dwie funkcje co nie powinno mieć miejsca (chyba że takie są założenia co sugeruje parametr printData) albo do rozbicia na ImportData i PrintData albo do uwzględnienia parametr
4. Parametr printData albo do usunięcia albo do wykorzystania aktualnie nigdzie nie jest wkorzystywany
5. Linia 16 => Inicjalizację tego parametru przenieść w miejsca utworzenia zmiennej coś w tym stylu (w jakim celu tworzymy element kolejki? => wydaje mi się iż do usunięcia) 
    IEnumerable<ImportedObject> ImportedObjects {get;set;} = new List<ImportedObject>(); 
6. Var streamReader = new StreamReader(fileToImport); powinno zostać wykorzystane w usingu by zwolnić zasoby po przetworzeniu danych
7. Clear and correct data wykonujemy na zmiennych warto to przenieść wcześniej by wykonać trim i replace na całych liniach np do lini 30
8. Jeśli IEnumerable<ImportedObjects> zamienimy na ICollection<ImportedObjects> nie będziemy musieli rzutować kolejki w lini 39
9. W jakim celu wykorzystujemy to ToUpper w lini 45? Jeśli po to by później porównywać to można wykorzystać string Equals z parameterm InvariantCultureIgnoreCase
10. Linie od 53 do 66 można zredukować za pomocą LINQ jeśli jednak ma zostać w ten sposób to napewno ify do zbicia w jeden warunek impObj.ParentType == importedObject.Type && impObj.ParentName == importedObject.Name
11. Linie od 68 do 98 warto wydzielić metody by poprawić czytelność kodu
12. Tekst "DATABASE" warto wynieść do zmiennej by móc z niej korzystać w innych częściach kodu i móc zarządzać tymi nazwami, idealnie byłoby stworzyć metodę która to sprawdza

Class ImportedObject oraz ImportedObjectBaseClass

1. Te klasy powinny znaleźć się w osobnych klasach
2. Warto trzymać się jednej konwencji więc wszystkie własności do poprawy na {get; set;} dodatkowo Name oraz ParentType do poprawy formatowania
3. IsNullable to string a powinno być bool jak nazwa wskazuje
4. NumberOfChildren warto poprawić na int gdyż double jest liczbą zmiennoprzecinkową co nie będzie miało miejsca w tym przypadku
5. Właśność Name do usunięcia gdyż dziediczyczmy z klasy która już tą wartość ma.
6. Własności ParentName i ParentType warto usunąć i zastąpić jedną właśnością o typie ImportedObjectBaseClass ParentName gdyż ma identyczne własności

Generalnie brakuje jakichkolwiek testów do tego kodu, dodatkowo zależności zewnętrzne takie jak StreamReader lub Console powinny zostać wydzielone i zastąpione interfejsami
w celu możliwości przetestowania tego kodu.

Większość zmian tutaj wypisanych uwzględniłem w zaproponowanym rozwiązaniu.