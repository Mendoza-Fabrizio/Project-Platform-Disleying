using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech; // Importar la librería para reconocimiento de voz

public class SpeechRecognitionTest : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textToRecognize;

    private DictationRecognizer dictationRecognizer;

    private void Start()
    {
        // Configurar los botones para iniciar y detener la grabación
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        stopButton.interactable = false;

        // Inicializar el DictationRecognizer
        dictationRecognizer = new DictationRecognizer();

        // Suscribirse al evento cuando se reconoce una frase
        dictationRecognizer.DictationResult += OnDictationResult;

        // Suscribirse a los eventos de error o finalización
        dictationRecognizer.DictationHypothesis += OnDictationHypothesis;
        dictationRecognizer.DictationComplete += OnDictationComplete;
        dictationRecognizer.DictationError += OnDictationError;
    }

    private void StartRecording()
    {
        text.color = Color.black;
        text.text = "Listening...";
        startButton.interactable = false;
        stopButton.interactable = true;

        // Iniciar el dictado
        dictationRecognizer.Start();
    }

    private void StopRecording()
    {
        text.color = Color.yellow;
        text.text = "Processing...";
        stopButton.interactable = false;

        // Detener el dictado
        dictationRecognizer.Stop();
    }

    private void OnDictationResult(string recognizedText, ConfidenceLevel confidence)
    {
        // Comparar el texto reconocido con el texto en 'textToRecognize'
        if (recognizedText.Equals(textToRecognize.text, System.StringComparison.OrdinalIgnoreCase))
        {
            text.color = Color.green;
            text.text = "Correct! You said: " + recognizedText;
        }
        else
        {
            text.color = Color.red;
            text.text = "Incorrect. You said: " + recognizedText;
        }

        // Habilitar el botón de inicio de grabación
        startButton.interactable = true;
    }

    private void OnDictationHypothesis(string hypothesis)
    {
        // Muestra una hipótesis mientras el usuario habla
        text.color = Color.cyan;
        text.text = "Hypothesis: " + hypothesis;
    }

    private void OnDictationComplete(DictationCompletionCause cause)
    {
        if (cause != DictationCompletionCause.Complete)
        {
            text.color = Color.red;
            text.text = "Dictation completed unsuccessfully: " + cause;
        }
    }

    private void OnDictationError(string error, int hresult)
    {
        text.color = Color.red;
        text.text = "Dictation error: " + error;
        startButton.interactable = true;
    }

    private void OnDestroy()
    {
        // Limpiar los eventos para evitar memory leaks
        dictationRecognizer.DictationResult -= OnDictationResult;
        dictationRecognizer.DictationHypothesis -= OnDictationHypothesis;
        dictationRecognizer.DictationComplete -= OnDictationComplete;
        dictationRecognizer.DictationError -= OnDictationError;

        dictationRecognizer.Dispose();
    }
}
