# Korp.Qualidade.InspecaoentradaFrontend

Alterar a flag IS_ON_PREMISE no jenkinsfile para true/false dependendo da sua branch:

- `true`: Caso a branch seja release/20xx.x.x.x_onpremise.
- `false`: Caso a branch seja release/20xx.x.x.x.

Alterar `updatePortalInterno` no arquivo config.yaml para falso caso seu projeto não seja publicado no portal interno.

Check for TODO in your workspace folder, and fix them.

Não esqueça de pedir a um membro do SDK a key para o fontawesome, insira essa key dentro do arquivo .npmrc e rode o seguinte comando ``` npm i -S @fortawesome/fontawesome-pro@^5.15.0 ```
