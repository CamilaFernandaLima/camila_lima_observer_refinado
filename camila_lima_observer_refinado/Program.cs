using System;
using System.Collections.Generic;

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
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }
}

public interface IObserver
{
    void Update(Subject s);
}

public class PCD : Subject
{
    public string nomeRio { get; set; }

    private double ph;
    private double temperatura;
    private double umidade;
    private double pressao;

    public PCD(string nome)
    {
        this.nomeRio = nome;
    }
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
        NotifyObservers();
    }
    public double GetTemperatura()
    {
        return temperatura;
    }
    public void SetTemperatura(double temperatura)
    {
        this.temperatura = temperatura;
        NotifyObservers();
    }
    public double GetUmidade()
    {
        return umidade;
    }
    public void SetUmidade(double umidade)
    {
        this.umidade = umidade;
        NotifyObservers();
    }
    public double GetPressao()
    {
        return pressao;
    }
    public void SetPressao(double pressao)
    {
        this.pressao = pressao;
        NotifyObservers();
    }
}

public class Universidade : IObserver
{
    private string nomeUni;
    public Universidade(string nome)
    {
        this.nomeUni = nome;
    }

    public void Update(Subject s)
    {
        PCD pcd = (PCD)s;
        Console.WriteLine($"[NOTIFICAÇÃO] {nomeUni} detectou mudança no {pcd.GetNomeRio()}.");
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