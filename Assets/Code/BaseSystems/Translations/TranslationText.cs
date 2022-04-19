using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Code.BaseSystems.Translations
{
    public class TranslationText : MonoBehaviour
    {
        [SerializeField] private string translationId;

        private Text standardText;
        private TextMeshProUGUI tmProText;
        
        private void OnDestroy()
        {
            TranslationsManager.Instance.OnLanguageChange -= SetText;
        }

        private void Start()
        {
            TranslationsManager.Instance.OnLanguageChange += SetText;
            standardText = GetComponent<Text>();
            tmProText = GetComponent<TextMeshProUGUI>();
            SetText();
        }

        private void SetText()
        {
            var t = TranslationsManager.Instance.GetPhrase(translationId);
            if(standardText!=null)
            {
                standardText.text = t;
            }
            else if(tmProText!=null)
            {
                tmProText.text = t;
            }
        }
    }
}