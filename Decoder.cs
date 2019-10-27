using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cipher_Decoder
{
    public class Decoder
    {
        private ulong _counter = 0;
        private HashSet<String> _document;
        private HashSet<String> _dictionary;
        private String _filename;
        private String _dictionaryName;
        private char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
        private char[] _decodedAlphabet;
        public Decoder (String filename, String dictionaryName) {
            this._filename = filename;
            this._dictionaryName = dictionaryName;
            this._document = ReadFromFile(this._filename);
            this._dictionary = ReadFromFile(this._dictionaryName);
            var strAlphabet = new String(_alphabet); 
            Permutation(strAlphabet, 0, strAlphabet.Length - 1);
        }

        private bool Check(char[] newAlphabet) {
            foreach (String word in _document) {
                String newWord = ApplyNewAlphabet(word, newAlphabet);
                if (!_dictionary.Contains(newWord)) {
                    return false;
                }
            }
            return true;
        }
        public void Permutation(String alphabet, int left, int right) {
            ++_counter;
            if(left == right) {
                var decodedAlphabet = alphabet.ToCharArray();
                if(Check(decodedAlphabet)) {
                    this._decodedAlphabet = decodedAlphabet;
                }
            } else {
                for (int i = left; i <= right; ++i) {
                    alphabet = SwapChars(alphabet, left, i);
                    Permutation(alphabet, left + 1, right);
                    alphabet = SwapChars(alphabet, left, i);
                }
            }
        }
        public String SwapChars(String alphabet, int direction, int index) {
            char temp;
            char[] alphabetArr = alphabet.ToCharArray();
            temp = alphabetArr[direction];
            alphabetArr[direction] = alphabetArr[index];
            alphabetArr[index] = temp;
            return new String(alphabetArr);
        }
        private String ApplyNewAlphabet(String word, char[] newAlphabet) {
            var newWord = new StringBuilder(word);
            var currentMapping = GetMapping(newAlphabet);
            for(int i = 0; i < word.Length; ++i) {
                newWord[i] = currentMapping[word[i]];
            }
            return newWord.ToString();
        }
        private Dictionary<char, char> GetMapping(char[] newAlphabet) {
            var mapping = new Dictionary<char, char>();
            for(int i = 0; i < _alphabet.Length; ++i) {
                mapping.Add(_alphabet[i], newAlphabet[i]);
            }
            return mapping;
        }
        private HashSet<String> ReadFromFile(String filename)
        {
            var wordSet = new HashSet<String>();
            using (var sr = new StreamReader(filename)) {
                String[] words = sr.ReadToEnd().Replace(Environment.NewLine, " ").Split(' ');
                foreach (var word in words) {
                    wordSet.Add(word.ToLower());
                }
            }
            return wordSet;
        }
        public void WriteDecodedDocument(String filename) {
            var decodedDocument = new StringBuilder();
            foreach (String word in this._document) {
                if(decodedDocument.Length != 0) {
                    decodedDocument.Append(" ").Append(ApplyNewAlphabet(word, this._decodedAlphabet));
                } else {
                    decodedDocument.Append(ApplyNewAlphabet(word, this._decodedAlphabet));
                }
            }
            using (var sw = new StreamWriter(filename)) {
                decodedDocument.Append(Environment.NewLine).Append("EncryptedAlphabet: ").Append(new String(this._decodedAlphabet));
                sw.WriteLine(decodedDocument.ToString());
            }
        }
        public HashSet<String> GetDocument () {
            return this._document;
        }
    }
}