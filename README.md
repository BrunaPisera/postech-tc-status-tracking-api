<div align="center">
<img src="https://github.com/user-attachments/assets/208a0ebb-ca7c-4b0b-9f68-0b35050a9880" width="30%" />
</div>

# VidProc- Video Status Tracking API (POS TECH: TECH CHALLENGE - FASE FINAL)🚀

Seja bem vindo! Este é um desafio proposto pela PósTech (Fiap + Alura) na ultima fase da pós graduação de Software Architecture (8SOAT).

📼 Vídeo de demonstração do projeto desta fase: em produção

Integrantes do grupo:<br>
Alexis Cesar (RM 356558)<br>
Bruna Gonçalves (RM 356557)

A Video Status Tracking API é responsável por acompanhar o status do processamento de vídeos. Ela consome mensagens do RabbitMQ quando o processamento é concluído, atualiza o status no banco de dados (Amazon RDS) e expõe um endpoint para que o cliente possa consultar o link das imagens geradas a partir do vídeo.

A aplicação é containerizada utilizando Docker, orquestrada por Kubernetes (K8s) para garantir escalabilidade e resiliência, e gerenciada por Helm, que automatiza o deployment e rollbacks no cluster Kubernetes (EKS) na nuvem da AWS.

## Navegação
- [Arquitetura](#arquitetura)
- [Fluxo](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Arquitetura

A aplicação segue a Arquitetura Limpa, que promove a separação de responsabilidade, facilitando a manutenção e escalabilidade. Esta abordagem permite que a lógica de negócios principal seja independente de qualquer dependência externa, como bancos de dados ou serviços externos.

## Fluxo
 
1. O serviço consome uma mensagem do RabbitMQ informando que o vídeo foi processado.
2. O status é atualizado no banco de dados (Amazon RDS).
3. O link com as imagens geradas é salvo.
4. Quando solicitado via API, o serviço retorna o status e o link correspondente

## Tecnologias Utilizadas
 
- **API Gateway**: Exposição da API.
- **Amazon S3**: Armazenamento de vídeos.
- **RabbitMQ (via CloudAMQP)**: Fila de mensagens para notificar o processamento.
- **Kubernetes**: Orquestração de contêineres.
- **Helm**: Gerenciamento de de pacotes kubernetes.
- **EKS**: Cluster Kubernetes na nuvem da AWS.
- **Terraform**: Automação de criação de recursos em provedores de nuvem.
