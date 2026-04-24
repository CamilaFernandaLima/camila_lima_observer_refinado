using System;
using System.Collections.Generic;


/* O modelo escolhido implementa a Inversão de COntrole (IoC), onde a classe Subject (PCD) não depende de classes 
 * concretas de observadores. Ela mantém apenas referência a uma interface (IObserver), que deve ser implementada 
 * por essas. 
 * Assim, o Subject decide o momento e o conteúdo da notificação, e executa o callback (Update) em cada observador 
 * registrado, mas não controla o que os observadores fazem com essa notificação -> Princípio de Hollywood.
 */

public abstract class Subject
{
    private List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
    public void NotifyObservers(PCD pcd, string tipoAlteracao)
    {
        foreach (var observer in observers)
        {
            // o callback, de fato, é a execução da interface (Subject chama a implementação definida pelo observador). 
            // a inversão de controle acontece aqui, pois o Subject não controla o que os Observers fazem com a notificação, apenas o momento e o conteúdo delas.
            observer.Update(pcd, tipoAlteracao);
        }
    }
}

// agora, essa interface define o contrato com os observadores
public interface IObserver
{
    void Update(PCD pcd, string tipoAlteracao);
}

public class PCD : Subject
{
    public string nomeRio { get; private set; }

    private double ph;
    private double temperatura;
    private double umidade;
    private double pressao;

    public PCD(string nome)
    {
        this.nomeRio = nome;
    }

    // a partir de agora, cada setter dispara um evento específico
    // ao chamar NotifyObservers, estamos empiurrando (push) a atualização para o observador, permitindo o callback.

    public string GetNomeRio()
    {
        return nomeRio;
    }
    public double GetPh()
    {
        return ph;
    }
    public void SetPh(double ph)
    {
        this.ph = ph;
        NotifyObservers(this, "ph");
    }
    public double GetTemperatura()
    {
        return temperatura;
    }
    public void SetTemperatura(double temperatura)
    {
        this.temperatura = temperatura;
        NotifyObservers(this, "temperatura");
    }
    public double GetUmidade()
    {
        return umidade;
    }
    public void SetUmidade(double umidade)
    {
        this.umidade = umidade;
        NotifyObservers(this, "umidade");
    }
    public double GetPressao()
    {
        return pressao;
    }
    public void SetPressao(double pressao)
    {
        this.pressao = pressao;
        NotifyObservers(this, "pressao");
    }
}

public class Universidade : IObserver
{
    private string nomeUni;
    public Universidade(string nome)
    {
        this.nomeUni = nome;
    }

    // CALLBACK: define a reação da Universidade à notificação de mudança de estado na PCD.
    // esse método computa a manifestação da notificação de mudança de estado e, portanto, permite que a Universidade reaja ao evento quando a PCD decide notificar.
    public void Update(PCD pcd, string tipoAlteracao)
    {
        Console.WriteLine($"[CALLBACK] {nomeUni} notificada de mudança em {tipoAlteracao} no {pcd.nomeRio}.");
        Console.WriteLine($"Temp: {pcd.GetTemperatura()}°C, pH: {pcd.GetPh()}, Umidade: {pcd.GetUmidade()}%, Pressão: {pcd.GetPressao()} hPa\n");
    }
}

public class Program
{
    public static void Main()
    {
        // 1. instancia universidades e rios
        List<Universidade> universidades = new List<Universidade>
        {
            new Universidade("UFSC"),
            new Universidade("UFPR"),
            new Universidade("UNIFESP"),
            new Universidade("USP"),
            new Universidade("UNICAMP"),
            new Universidade("UFRJ"),
            new Universidade("UFMG"),
            new Universidade("UFV")
        };

        List<PCD> rios = new List<PCD>
        {
            new PCD("Rio Amazonas"),
            new PCD("Rio Negro"),
            new PCD("Rio Solimões"),
            new PCD("Rio Madeira"),
            new PCD("Rio Tapajós"),
        };

        // 2. universidades se tornam observadoras dos rios

        //USP e UNIFESP monitoram o Rio Amazonas
        rios[0].AddObserver(universidades[2]);
        rios[0].AddObserver(universidades[3]);

        //UFSC e UFRJ monitoram o Rio Negro
        rios[1].AddObserver(universidades[0]);
        rios[1].AddObserver(universidades[5]);

        //UFPR monitora o Rio Solimões
        rios[2].AddObserver(universidades[1]);

        //UNICAMP e UFMG monitoram o Rio Madeira
        rios[3].AddObserver(universidades[4]);
        rios[3].AddObserver(universidades[6]);

        //UFV monitora o Rio Tapajós
        rios[4].AddObserver(universidades[7]);

        // 3. simulação das mudanças nas PCDs
        Console.WriteLine("Sistema de monitoramento de rios da Amazônia:\n");

        rios[0].SetPh(6.5);
        rios[1].SetTemperatura(28.0);
        rios[2].SetUmidade(85.0);
        rios[3].SetPressao(1013.0);
        rios[4].SetTemperatura(22.0);
        rios[2].SetTemperatura(25.0);
    }
}